import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap, throwError } from 'rxjs';
import { AllegroOrdersStateModel } from './allegro-orders.state.model';
import { AllegroOrderModel } from '../models/allegro-order.model';
import { AllegroOrdersService } from '../services/allegro-orders.service';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import { ChangePage, Load } from './allegro-orders.action';

const TEST_STATE_TOKEN = new StateToken<AllegroOrdersStateModel>('allegro_orders');

@State<AllegroOrdersStateModel>({
  name: TEST_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<AllegroOrderModel[]>(),
  },
})
@Injectable()
export class AllegroOrdersState {
  constructor(private logsService: AllegroOrdersService) {}

  @Selector([TEST_STATE_TOKEN])
  static getOrders(state: AllegroOrdersStateModel): AllegroOrderModel[] {
    return state.restQueryResponse.result;
  }

  @Selector([TEST_STATE_TOKEN])
  static getOrdersCount(state: AllegroOrdersStateModel): number {
    return state.restQueryResponse.totalCount;
  }

  @Selector([TEST_STATE_TOKEN])
  static getCurrentPage(state: AllegroOrdersStateModel): number {
    return state.restQuery.currentPage.pageIndex;
  }

  @Selector([TEST_STATE_TOKEN])
  static getPageSize(state: AllegroOrdersStateModel): number {
    return state.restQuery.currentPage.pageSize;
  }

  @Action(Load)
  loadLogs(ctx: StateContext<AllegroOrdersStateModel>, _: Load) {
    return this.logsService.load(ctx.getState().restQuery.currentPage).pipe(
      tap(response => {
        const customResponse = new RestQueryResponse<AllegroOrderModel[]>();
        customResponse.result = response.orders;
        customResponse.totalCount = response.ordersCount;

        ctx.patchState({
          restQueryResponse: customResponse,
        });
      }),
      catchError(error => {
        return throwError(error);
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
}
