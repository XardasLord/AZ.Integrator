import { Injectable, NgZone } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, of, switchMap, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AllegroOrdersStateModel } from './allegro-orders.state.model';
import { AllegroOrdersService } from '../services/allegro-orders.service';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import {
  ChangePage,
  GenerateInpostLabel,
  LoadNew,
  LoadShipments,
  OpenRegisterInPostShipmentModal,
  RegisterInpostShipment,
  LoadReadyForShipment,
  LoadSent,
  OpenRegisterDpdShipmentModal,
  RegisterDpdShipment,
  GenerateDpdLabel,
} from './allegro-orders.action';
import { RegisterParcelModalComponent } from '../pages/register-parcel-modal/register-parcel-modal.component';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';
import { IntegratorQueryShipmentsArgs, ShipmentViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { DownloadService } from '../../../shared/services/download.service';
import { AllegroOrderFulfillmentStatusEnum } from '../models/allegro-order-fulfillment-status.enum';
import { RegisterShipmentDataModel } from '../models/register-shipment-data.model';
import { HttpErrorResponse } from '@angular/common/http';
import { IntegratorError } from '../../../core/interceptor/error-handler.interceptor';
import { ShipmentProviderEnum } from '../models/shipment-provider.enum';
import { GetAllegroOrdersResponseModel } from '../models/get-allegro-orders-response.model';

const ALLEGRO_ORDERS_STATE_TOKEN = new StateToken<AllegroOrdersStateModel>('allegro_orders');

@State<AllegroOrdersStateModel>({
  name: ALLEGRO_ORDERS_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<AllegroOrderDetailsModel[]>(),
    selectedOrderDetails: null,
    shipments: [],
  },
})
@Injectable()
export class AllegroOrdersState {
  private dialogRef?: MatDialogRef<RegisterParcelModalComponent>;

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
  static getOrdersWithRegisteredShipmentCount(state: AllegroOrdersStateModel): number {
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
    return this.allegroOrderService
      .load(ctx.getState().restQuery.currentPage, AllegroOrderFulfillmentStatusEnum.New)
      .pipe(
        tap(response => {
          this.handleAllegroOrdersResponse(ctx, response);
        })
      );
  }

  @Action(LoadReadyForShipment)
  loadReadyForShipmentOrders(ctx: StateContext<AllegroOrdersStateModel>) {
    return this.allegroOrderService
      .load(ctx.getState().restQuery.currentPage, AllegroOrderFulfillmentStatusEnum.ReadyForShipment)
      .pipe(
        tap(response => {
          this.handleAllegroOrdersResponse(ctx, response);
        })
      );
  }

  @Action(LoadSent)
  loadSentOrders(ctx: StateContext<AllegroOrdersStateModel>) {
    return this.allegroOrderService
      .load(ctx.getState().restQuery.currentPage, AllegroOrderFulfillmentStatusEnum.Sent)
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

    // TODO: Depends on the currently opened tab (New/ReadyForShipment/Sent)
    return ctx.dispatch(new LoadNew());
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

      this.dialogRef = this.dialog.open<RegisterParcelModalComponent, RegisterShipmentDataModel>(
        RegisterParcelModalComponent,
        {
          data: <RegisterShipmentDataModel>data,
          width: '50%',
          height: '75%',
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

      this.dialogRef = this.dialog.open<RegisterParcelModalComponent, RegisterShipmentDataModel>(
        RegisterParcelModalComponent,
        {
          data: <RegisterShipmentDataModel>data,
          width: '50%',
          height: '75%',
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
}
