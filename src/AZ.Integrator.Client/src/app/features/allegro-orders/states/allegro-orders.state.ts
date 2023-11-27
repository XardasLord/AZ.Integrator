import { Injectable, NgZone } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { HttpErrorResponse } from '@angular/common/http';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, Observable, of, switchMap, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AllegroOrdersStateModel } from './allegro-orders.state.model';
import { AllegroOrdersService } from '../services/allegro-orders.service';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import {
  ApplyFilter,
  ChangePage,
  GenerateDpdLabel,
  GenerateInpostLabel,
  LoadNew,
  LoadReadyForShipment,
  LoadSent,
  LoadShipments,
  OpenRegisterDpdShipmentModal,
  OpenRegisterInPostShipmentModal,
  RegisterDpdShipment,
  RegisterInpostShipment,
  SetCurrentTab,
} from './allegro-orders.action';
import { RegisterShipmentModalComponent } from '../pages/register-shipment-modal/register-shipment-modal.component';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';
import { IntegratorQueryShipmentsArgs, ShipmentViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { DownloadService } from '../../../shared/services/download.service';
import { AllegroOrderFulfillmentStatusEnum } from '../models/allegro-order-fulfillment-status.enum';
import { RegisterShipmentDataModel } from '../models/register-shipment-data.model';
import { IntegratorError } from '../../../core/interceptor/error-handler.interceptor';
import { ShipmentProviderEnum } from '../models/shipment-provider.enum';
import { GetAllegroOrdersResponseModel } from '../models/get-allegro-orders-response.model';
import { LoadInvoices } from './invoices.action';

const ALLEGRO_ORDERS_STATE_TOKEN = new StateToken<AllegroOrdersStateModel>('allegro_orders');

@State<AllegroOrdersStateModel>({
  name: ALLEGRO_ORDERS_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<AllegroOrderDetailsModel[]>(),
    selectedOrderDetails: null,
    shipments: [],
    currentTab: 'New',
  },
})
@Injectable()
export class AllegroOrdersState {
  private dialogRef?: MatDialogRef<RegisterShipmentModalComponent>;

  constructor(
    private allegroOrderService: AllegroOrdersService,
    private downloadService: DownloadService,
    private dialog: MatDialog,
    private zone: NgZone,
    private toastService: ToastrService
  ) {}

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getAllNewOrders(state: AllegroOrdersStateModel): AllegroOrderDetailsModel[] {
    return state.restQueryResponse.result;
  }

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getAllNewOrdersCount(state: AllegroOrdersStateModel): number {
    return state.restQueryResponse.totalCount;
  }

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getNotFinishedOrdersWithRegisteredShipmentCount(state: AllegroOrdersStateModel): number {
    return state.restQueryResponse.totalCount;
  }

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getCurrentPage(state: AllegroOrdersStateModel): number {
    return state.restQuery.currentPage.pageIndex;
  }

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getPageSize(state: AllegroOrdersStateModel): number {
    return state.restQuery.currentPage.pageSize;
  }

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getSearchText(state: AllegroOrdersStateModel): string {
    return state.restQuery.searchText;
  }

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getShipments(state: AllegroOrdersStateModel): ShipmentViewModel[] {
    return state.shipments;
  }

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getInpostShipments(state: AllegroOrdersStateModel): ShipmentViewModel[] {
    return state.shipments.filter(x => x.shipmentProvider === ShipmentProviderEnum.Inpost);
  }

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getDpdShipments(state: AllegroOrdersStateModel): ShipmentViewModel[] {
    return state.shipments.filter(x => x.shipmentProvider === ShipmentProviderEnum.Dpd);
  }

  @Action(LoadNew)
  loadNewOrders(ctx: StateContext<AllegroOrdersStateModel>) {
    const query = ctx.getState().restQuery;

    return this.allegroOrderService
      .load(query.currentPage, [AllegroOrderFulfillmentStatusEnum.Processing], query.searchText)
      .pipe(
        tap(response => {
          this.handleAllegroOrdersResponse(ctx, response);
        })
      );
  }

  @Action(LoadReadyForShipment)
  loadReadyForShipmentOrders(ctx: StateContext<AllegroOrdersStateModel>) {
    const query = ctx.getState().restQuery;

    return this.allegroOrderService
      .load(query.currentPage, [AllegroOrderFulfillmentStatusEnum.ReadyForShipment], query.searchText)
      .pipe(
        tap(response => {
          this.handleAllegroOrdersResponse(ctx, response);
          ctx.dispatch(new LoadInvoices(response.orders.map(x => x.id)));
        })
      );
  }

  @Action(LoadSent)
  loadSentOrders(ctx: StateContext<AllegroOrdersStateModel>) {
    const query = ctx.getState().restQuery;

    return this.allegroOrderService
      .load(query.currentPage, [AllegroOrderFulfillmentStatusEnum.Sent], query.searchText)
      .pipe(
        tap(response => {
          this.handleAllegroOrdersResponse(ctx, response);
        })
      );
  }

  @Action(ChangePage)
  changePage(ctx: StateContext<AllegroOrdersStateModel>, action: ChangePage) {
    const customQuery = new RestQueryVo();
    customQuery.currentPage = action.event;

    ctx.patchState({
      restQuery: customQuery,
    });

    return this.loadData(ctx);
  }

  @Action(ApplyFilter)
  applyFilter(ctx: StateContext<AllegroOrdersStateModel>, action: ApplyFilter): Observable<void> {
    const updatedQuery = { ...ctx.getState().restQuery };
    updatedQuery.searchText = action.searchPhrase;

    ctx.patchState({
      restQuery: updatedQuery,
    });

    return this.loadData(ctx);
  }

  @Action(SetCurrentTab)
  setCurrentTab(ctx: StateContext<AllegroOrdersStateModel>, action: SetCurrentTab) {
    ctx.patchState({
      currentTab: action.currentTab,
    });
  }

  @Action(LoadShipments)
  loadInpostShipments(ctx: StateContext<AllegroOrdersStateModel>, action: LoadShipments) {
    let filters: IntegratorQueryShipmentsArgs = {};

    if (action.allegroOrderIds.length > 0) {
      filters = {
        where: {
          allegroOrderNumber: {
            in: action.allegroOrderIds,
          },
        },
      };
    }
    return this.allegroOrderService.getShipments(filters).pipe(
      tap(shipmentsResponse => {
        ctx.patchState({
          shipments: shipmentsResponse.result,
        });
      })
    );
  }

  @Action(OpenRegisterInPostShipmentModal)
  openRegisterInPostShipmentModal(ctx: StateContext<AllegroOrdersStateModel>, action: OpenRegisterInPostShipmentModal) {
    this.zone.run(() => {
      const data: RegisterShipmentDataModel = {
        allegroOrder: action.order,
        deliveryMethodType: 'INPOST',
      };

      this.dialogRef = this.dialog.open<RegisterShipmentModalComponent, RegisterShipmentDataModel>(
        RegisterShipmentModalComponent,
        {
          data: <RegisterShipmentDataModel>data,
          width: '50%',
          height: '82%',
        }
      );
    });
  }

  @Action(OpenRegisterDpdShipmentModal)
  openRegisterDpdShipmentModal(ctx: StateContext<AllegroOrdersStateModel>, action: OpenRegisterInPostShipmentModal) {
    this.zone.run(() => {
      const data: RegisterShipmentDataModel = {
        allegroOrder: action.order,
        deliveryMethodType: 'DPD',
      };

      this.dialogRef = this.dialog.open<RegisterShipmentModalComponent, RegisterShipmentDataModel>(
        RegisterShipmentModalComponent,
        {
          data: <RegisterShipmentDataModel>data,
          width: '50%',
          height: '82%',
        }
      );
    });
  }

  @Action(RegisterInpostShipment)
  registerInpostShipment(ctx: StateContext<AllegroOrdersStateModel>, action: RegisterInpostShipment) {
    return this.allegroOrderService.registerInpostShipment(action.command).pipe(
      tap(() => {
        this.zone.run(() => this.toastService.success('Przesyłka została zarejestrowana w InPost', 'Przesyłka InPost'));
        this.dialogRef?.close();

        ctx.dispatch(new LoadShipments());
      }),
      catchError(error => {
        this.zone.run(() => this.toastService.error('Błąd podczas rejestrowania przesyłki Inpost', 'Przesyłka Inpost'));
        return throwError(error);
      })
    );
  }

  @Action(RegisterDpdShipment)
  registerDpdShipment(ctx: StateContext<AllegroOrdersStateModel>, action: RegisterDpdShipment) {
    return this.allegroOrderService.registerDpdShipment(action.command).pipe(
      tap(() => {
        this.zone.run(() => this.toastService.success('Przesyłka została zarejestrowana w DPD', 'Przesyłka DPD'));
        this.dialogRef?.close();

        ctx.dispatch(new LoadShipments());
      }),
      catchError((error: HttpErrorResponse) => {
        const errorDetails: IntegratorError = error.error;

        this.zone.run(() =>
          this.toastService.error(`Błąd podczas rejestrowania przesyłki DPD - ${errorDetails.Message}`, 'Przesyłka DPD')
        );
        return throwError(error);
      })
    );
  }

  @Action(GenerateInpostLabel)
  generateInpostLabel(ctx: StateContext<AllegroOrdersStateModel>, action: GenerateInpostLabel) {
    const shipmentNumber = ctx
      .getState()
      .shipments.filter(
        x => x.shipmentProvider === ShipmentProviderEnum.Inpost && x.allegroOrderNumber === action.allegroOrderNumber
      )[0].shipmentNumber!;

    return this.downloadService.downloadFileFromApi(`/inpostShipments/${shipmentNumber}/label`).pipe(
      switchMap(resBlob => {
        this.downloadService.getFile(resBlob, 'ShipmentLabel.pdf');
        this.toastService.success('List przewozowy został wygenerowany.', 'List przewozowy');

        return of(null);
      }),
      catchError(() => {
        this.toastService.error(`Błąd podczas pobierania raportu CSV`, 'Raport CSV');

        return of(null);
      })
    );
  }

  @Action(GenerateDpdLabel)
  generateDpdLabel(ctx: StateContext<AllegroOrdersStateModel>, action: GenerateDpdLabel) {
    const shipmentNumber = ctx
      .getState()
      .shipments.filter(
        x => x.shipmentProvider === ShipmentProviderEnum.Dpd && x.allegroOrderNumber === action.allegroOrderNumber
      )[0].shipmentNumber!;

    return this.downloadService.downloadFileFromApi(`/dpdShipments/${shipmentNumber}/label`).pipe(
      switchMap(resBlob => {
        this.downloadService.getFile(resBlob, 'ShipmentLabel.pdf');
        this.toastService.success('List przewozowy został wygenerowany.', 'List przewozowy');

        return of(null);
      }),
      catchError(() => {
        this.toastService.error(`Błąd podczas pobierania raportu CSV`, 'Raport CSV');

        return of(null);
      })
    );
  }

  private handleAllegroOrdersResponse(
    ctx: StateContext<AllegroOrdersStateModel>,
    response: GetAllegroOrdersResponseModel
  ) {
    ctx.dispatch(new LoadShipments(response.orders.map(x => x.id)));

    const customResponse = new RestQueryResponse<AllegroOrderDetailsModel[]>();
    customResponse.result = response.orders;
    customResponse.totalCount = response.totalCount;

    ctx.patchState({
      restQueryResponse: customResponse,
    });
  }

  private loadData(ctx: StateContext<AllegroOrdersStateModel>) {
    switch (ctx.getState().currentTab) {
      case 'New':
        return ctx.dispatch(new LoadNew());
      case 'ReadyForShipment':
        return ctx.dispatch(new LoadReadyForShipment());
      case 'Sent':
        return ctx.dispatch(new LoadSent());
    }
  }
}
