import { inject, Injectable, NgZone } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, Observable, of, switchMap, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { OrdersStateModel } from './orders-state.model';
import { OrdersService } from '../services/orders.service';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import {
  ApplyFilter,
  ChangePage,
  GenerateDpdLabel,
  GenerateInpostLabel,
  GenerateInpostLabels,
  LoadNew,
  LoadOrders,
  LoadReadyForShipment,
  LoadSent,
  LoadShipments,
  OpenRegisterDpdShipmentModal,
  OpenRegisterInPostShipmentModal,
  RegisterDpdShipment,
  RegisterInpostShipment,
  SetCurrentTab,
} from './orders.action';
import { RegisterShipmentModalComponent } from '../pages/register-shipment-modal/register-shipment-modal.component';
import { OrderDetailsModel } from '../models/order-details.model';
import { IntegratorQueryShipmentsArgs, ShipmentViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { DownloadService } from '../../../shared/services/download.service';
import { OrderFulfillmentStatusEnum } from '../models/order-fulfillment-status.enum';
import { RegisterShipmentDataModel } from '../models/register-shipment-data.model';
import { IntegratorError } from '../../../core/interceptor/error-handler.interceptor';
import { ShipmentProviderEnum } from '../models/shipment-provider.enum';
import { GetOrdersResponseModel } from '../models/get-orders-response.model';
import { LoadInvoices } from './invoices.action';
import { RestQueryHelper } from '../../../shared/models/pagination/rest.helper';

const ORDERS_STATE_TOKEN = new StateToken<OrdersStateModel>('orders');

@State<OrdersStateModel>({
  name: ORDERS_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<OrderDetailsModel[]>(),
    selectedOrderDetails: null,
    shipments: [],
    currentTab: 'New',
  },
})
@Injectable()
export class OrdersState {
  private orderService = inject(OrdersService);
  private downloadService = inject(DownloadService);
  private dialog = inject(MatDialog);
  private zone = inject(NgZone);
  private toastService = inject(ToastrService);

  private dialogRef?: MatDialogRef<RegisterShipmentModalComponent>;

  @Selector([ORDERS_STATE_TOKEN])
  static getAllNewOrders(state: OrdersStateModel): OrderDetailsModel[] {
    return state.restQueryResponse.result;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getAllNewOrdersCount(state: OrdersStateModel): number {
    return state.restQueryResponse.totalCount;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getNotFinishedOrdersWithRegisteredShipmentCount(state: OrdersStateModel): number {
    return state.restQueryResponse.totalCount;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getCurrentPage(state: OrdersStateModel): number {
    return state.restQuery.currentPage.pageIndex;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getPageSize(state: OrdersStateModel): number {
    return state.restQuery.currentPage.pageSize;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getSearchText(state: OrdersStateModel): string {
    return state.restQuery.searchText;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getShipments(state: OrdersStateModel): ShipmentViewModel[] {
    return state.shipments;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getInpostShipments(state: OrdersStateModel): ShipmentViewModel[] {
    return state.shipments.filter(x => x.shipmentProvider === ShipmentProviderEnum.Inpost);
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getDpdShipments(state: OrdersStateModel): ShipmentViewModel[] {
    return state.shipments.filter(x => x.shipmentProvider === ShipmentProviderEnum.Dpd);
  }

  @Action(LoadOrders)
  loadOrders(ctx: StateContext<OrdersStateModel>) {
    return this.loadData(ctx);
  }

  @Action(LoadNew)
  loadNewOrders(ctx: StateContext<OrdersStateModel>) {
    const query = ctx.getState().restQuery;

    return this.orderService.load(query.currentPage, [OrderFulfillmentStatusEnum.Processing], query.searchText).pipe(
      tap(response => {
        this.handleOrdersResponse(ctx, response);
      })
    );
  }

  @Action(LoadReadyForShipment)
  loadReadyForShipmentOrders(ctx: StateContext<OrdersStateModel>) {
    const query = ctx.getState().restQuery;

    return this.orderService
      .load(query.currentPage, [OrderFulfillmentStatusEnum.ReadyForShipment], query.searchText)
      .pipe(
        tap(response => {
          this.handleOrdersResponse(ctx, response);
          ctx.dispatch(new LoadInvoices(response.orders.map(x => x.id)));
        })
      );
  }

  @Action(LoadSent)
  loadSentOrders(ctx: StateContext<OrdersStateModel>) {
    const query = ctx.getState().restQuery;

    return this.orderService.load(query.currentPage, [OrderFulfillmentStatusEnum.Sent], query.searchText).pipe(
      tap(response => {
        this.handleOrdersResponse(ctx, response);
        ctx.dispatch(new LoadInvoices(response.orders.map(x => x.id)));
      })
    );
  }

  @Action(ChangePage)
  changePage(ctx: StateContext<OrdersStateModel>, action: ChangePage) {
    const customQuery = new RestQueryVo();
    customQuery.currentPage = action.event;

    ctx.patchState({
      restQuery: customQuery,
    });

    return this.loadData(ctx);
  }

  @Action(ApplyFilter)
  applyFilter(ctx: StateContext<OrdersStateModel>, action: ApplyFilter): Observable<void> {
    const updatedQuery = { ...ctx.getState().restQuery };
    updatedQuery.searchText = action.searchPhrase;
    updatedQuery.currentPage = RestQueryHelper.getInitialPageEvent();

    ctx.patchState({
      restQuery: updatedQuery,
    });

    return this.loadData(ctx);
  }

  @Action(SetCurrentTab)
  setCurrentTab(ctx: StateContext<OrdersStateModel>, action: SetCurrentTab) {
    ctx.patchState({
      currentTab: action.currentTab,
    });
  }

  @Action(LoadShipments)
  loadInpostShipments(ctx: StateContext<OrdersStateModel>, action: LoadShipments) {
    let filters: IntegratorQueryShipmentsArgs = {};

    if (action.orderIds.length > 0) {
      filters = {
        where: {
          externalOrderNumber: {
            in: action.orderIds,
          },
        },
      };
    }
    return this.orderService.getShipments(filters).pipe(
      tap(shipmentsResponse => {
        ctx.patchState({
          shipments: shipmentsResponse.result,
        });
      })
    );
  }

  @Action(OpenRegisterInPostShipmentModal)
  openRegisterInPostShipmentModal(ctx: StateContext<OrdersStateModel>, action: OpenRegisterInPostShipmentModal) {
    this.zone.run(() => {
      const data: RegisterShipmentDataModel = {
        order: action.order,
        deliveryMethodType: 'INPOST',
      };

      this.dialogRef = this.dialog.open<RegisterShipmentModalComponent, RegisterShipmentDataModel>(
        RegisterShipmentModalComponent,
        {
          data: <RegisterShipmentDataModel>data,
          width: '60%',
          height: '82%',
        }
      );
    });
  }

  @Action(OpenRegisterDpdShipmentModal)
  openRegisterDpdShipmentModal(ctx: StateContext<OrdersStateModel>, action: OpenRegisterInPostShipmentModal) {
    this.zone.run(() => {
      const data: RegisterShipmentDataModel = {
        order: action.order,
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
  registerInpostShipment(ctx: StateContext<OrdersStateModel>, action: RegisterInpostShipment) {
    return this.orderService.registerInpostShipment(action.command).pipe(
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
  registerDpdShipment(ctx: StateContext<OrdersStateModel>, action: RegisterDpdShipment) {
    return this.orderService.registerDpdShipment(action.command).pipe(
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
  generateInpostLabel(ctx: StateContext<OrdersStateModel>, action: GenerateInpostLabel) {
    const shipmentNumber = ctx
      .getState()
      .shipments.filter(
        x => x.shipmentProvider === ShipmentProviderEnum.Inpost && x.externalOrderNumber === action.orderNumber
      )[0].shipmentNumber!;

    return this.downloadService.downloadFileFromApi(`/inpostShipments/${shipmentNumber}/label`).pipe(
      switchMap(resBlob => {
        this.downloadService.getFile(resBlob, 'ShipmentLabel.pdf');
        this.toastService.success('List przewozowy został wygenerowany.', 'List przewozowy');

        return of(null);
      }),
      catchError(() => {
        this.toastService.error(`Błąd podczas pobierania listu przewozowego`, 'List przewozowy');

        return of(null);
      })
    );
  }

  @Action(GenerateInpostLabels)
  generateInpostLabels(ctx: StateContext<OrdersStateModel>, action: GenerateInpostLabels) {
    const shipmentNumbers = ctx
      .getState()
      .shipments.filter(
        x => x.shipmentProvider === ShipmentProviderEnum.Inpost && action.orderNumbers.includes(x.externalOrderNumber!)
      )
      .map(x => x.shipmentNumber!);

    let params = new HttpParams();

    shipmentNumbers.forEach(shipmentNumber => {
      params = params.append('shipmentNumber', shipmentNumber);
    });

    return this.downloadService.downloadFileFromApi(`/inpostShipments/label`, params).pipe(
      switchMap(resBlob => {
        this.downloadService.getFile(resBlob, 'ShipmentLabel.zip');
        this.toastService.success('Listy przewozowe zostały wygenerowane.', 'Listy przewozowe');

        return of(null);
      }),
      catchError(() => {
        this.toastService.error(`Błąd podczas pobierania listów przewozowych`, 'Listy przewozowe');

        return of(null);
      })
    );
  }

  @Action(GenerateDpdLabel)
  generateDpdLabel(ctx: StateContext<OrdersStateModel>, action: GenerateDpdLabel) {
    const shipmentNumber = ctx
      .getState()
      .shipments.filter(
        x => x.shipmentProvider === ShipmentProviderEnum.Dpd && x.externalOrderNumber === action.orderNumber
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

  private handleOrdersResponse(ctx: StateContext<OrdersStateModel>, response: GetOrdersResponseModel) {
    ctx.dispatch(new LoadShipments(response.orders.map(x => x.id)));

    const customResponse = new RestQueryResponse<OrderDetailsModel[]>();
    customResponse.result = response.orders;
    customResponse.totalCount = response.totalCount;

    ctx.patchState({
      restQueryResponse: customResponse,
    });
  }

  private loadData(ctx: StateContext<OrdersStateModel>) {
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
