import { Injectable, NgZone } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, of, switchMap, tap, throwError } from 'rxjs';
import { AllegroOrdersStateModel } from './allegro-orders.state.model';
import { AllegroOrdersService } from '../services/allegro-orders.service';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import {
  ChangePage,
  GenerateInpostLabel,
  Load,
  LoadInpostShipments,
  OpenRegisterInPostShipmentModal,
  RegisterInpostShipment,
} from './allegro-orders.action';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { RegisterParcelModalComponent } from '../pages/register-parcel-modal/register-parcel-modal.component';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';
import { ToastrService } from 'ngx-toastr';
import { InpostShipmentViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { DownloadService } from '../../../shared/services/download.service';

const ALLEGRO_ORDERS_STATE_TOKEN = new StateToken<AllegroOrdersStateModel>('allegro_orders');

@State<AllegroOrdersStateModel>({
  name: ALLEGRO_ORDERS_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<AllegroOrderDetailsModel[]>(),
    selectedOrderDetails: null,
    inpostShipments: [],
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
  static getInpostShipments(state: AllegroOrdersStateModel): InpostShipmentViewModel[] {
    return state.inpostShipments;
  }

  @Action(Load)
  loadOrders(ctx: StateContext<AllegroOrdersStateModel>) {
    return this.allegroOrderService.load(ctx.getState().restQuery.currentPage).pipe(
      tap(response => {
        const customResponse = new RestQueryResponse<AllegroOrderDetailsModel[]>();
        customResponse.result = response.orders;
        customResponse.totalCount = response.totalCount;

        ctx.patchState({
          restQueryResponse: customResponse,
        });
      })
    );
  }

  @Action(LoadInpostShipments)
  loadInpostShipments(ctx: StateContext<AllegroOrdersStateModel>) {
    return this.allegroOrderService.getInpostShipments({}).pipe(
      tap(shipmentsResponse => {
        ctx.patchState({
          inpostShipments: shipmentsResponse.result,
        });
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

    return ctx.dispatch(new Load());
  }

  @Action(OpenRegisterInPostShipmentModal)
  openRegisterInPostShipmentModal(ctx: StateContext<AllegroOrdersStateModel>, action: OpenRegisterInPostShipmentModal) {
    this.zone.run(() => {
      this.dialogRef = this.dialog.open<RegisterParcelModalComponent, AllegroOrderDetailsModel>(
        RegisterParcelModalComponent,
        {
          data: <AllegroOrderDetailsModel>action.order,
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

        ctx.dispatch(new LoadInpostShipments());
      }),
      catchError(error => {
        this.zone.run(() => this.toastService.error('Błąd podczas rejestrowania przesyłki Inpost', 'Przesyłka Inpost'));
        return throwError(error);
      })
    );
  }

  @Action(GenerateInpostLabel)
  generateInpostLabel(ctx: StateContext<AllegroOrdersStateModel>, action: GenerateInpostLabel) {
    const shipmentNumber = ctx
      .getState()
      .inpostShipments.filter(x => x.allegroOrderNumber === action.allegroOrderNumber)[0].inpostShipmentNumber!;

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
}
