import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap, throwError } from 'rxjs';
import { AllegroOrdersStateModel } from './allegro-orders.state.model';
import { AllegroOrderModel } from '../models/allegro-order.model';
import { AllegroOrdersService } from '../services/allegro-orders.service';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import { ChangePage, Load, OpenRegisterParcelModal } from './allegro-orders.action';
import { MatDialog } from '@angular/material/dialog';
import { RegisterParcelModalComponent } from '../pages/register-parcel-modal/register-parcel-modal.component';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';

const ALLEGRO_ORDERS_STATE_TOKEN = new StateToken<AllegroOrdersStateModel>('allegro_orders');

@State<AllegroOrdersStateModel>({
  name: ALLEGRO_ORDERS_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<AllegroOrderModel[]>(),
    selectedOrderDetails: null,
  },
})
@Injectable()
export class AllegroOrdersState {
  constructor(
    private allegroOrderService: AllegroOrdersService,
    private dialog: MatDialog
  ) {}

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getOrders(state: AllegroOrdersStateModel): AllegroOrderModel[] {
    return state.restQueryResponse.result;
  }

  @Selector([ALLEGRO_ORDERS_STATE_TOKEN])
  static getOrdersCount(state: AllegroOrdersStateModel): number {
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

  @Action(Load)
  loadOrders(ctx: StateContext<AllegroOrdersStateModel>) {
    return this.allegroOrderService.load(ctx.getState().restQuery.currentPage).pipe(
      tap(response => {
        const customResponse = new RestQueryResponse<AllegroOrderModel[]>();
        customResponse.result = response.orders;
        customResponse.totalCount = response.totalCount;

        ctx.patchState({
          restQueryResponse: customResponse,
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

  @Action(OpenRegisterParcelModal)
  registerParcel(ctx: StateContext<AllegroOrdersStateModel>, action: OpenRegisterParcelModal) {
    return this.allegroOrderService.getDetails(action.order.orderId).pipe(
      tap(response => {
        ctx.patchState({
          selectedOrderDetails: response,
        });

        this.dialog.open<RegisterParcelModalComponent, AllegroOrderDetailsModel>(RegisterParcelModalComponent, {
          data: <AllegroOrderDetailsModel>response,
        });
      })
    );
  }
}
