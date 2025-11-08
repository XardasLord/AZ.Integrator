import { inject, Injectable, NgZone } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { tap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { OrdersStateModel } from './orders.state.model';
import { OrdersService } from '../services/orders.service';
import { ApplyFilter, ChangePage, CreateOrder, LoadOrders } from './orders.action';
import { GraphQLQueryVo } from '../../../../shared/graphql/graphql.query';
import { GraphQLResponse } from '../../../../shared/graphql/graphql.response';
import { applyChangePageFilter, applyCommonSearchFilter } from '../../../../shared/graphql/common-search-filter';
import {
  IntegratorQueryPartDefinitionOrdersArgs,
  PartDefinitionsOrderViewModel,
  PartDefinitionsOrderViewModelSortInput,
  SortEnumType,
} from '../../../../shared/graphql/graphql-integrator.schema';
import { Navigate } from '@ngxs/router-plugin';
import { FurnitureFormatsRoutePath } from '../../../../core/modules/app-routing.module';

const ORDERS_STATE_TOKEN = new StateToken<OrdersStateModel>('furnitureOrders');

@State<OrdersStateModel>({
  name: ORDERS_STATE_TOKEN,
  defaults: {
    orders: [],
    graphQLQuery: new GraphQLQueryVo(),
    graphQLResponse: new GraphQLResponse<PartDefinitionsOrderViewModel[]>(),
    graphQLFilters: {
      order: [{ id: SortEnumType.Desc }],
    },
  },
})
@Injectable()
export class OrdersState {
  private ordersService = inject(OrdersService);
  private zone = inject(NgZone);
  private toastService = inject(ToastrService);

  @Selector([ORDERS_STATE_TOKEN])
  static getOrders(state: OrdersStateModel): PartDefinitionsOrderViewModel[] {
    return state.orders;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getTotalCount(state: OrdersStateModel): number {
    return state.graphQLResponse?.result?.totalCount ?? 0;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getCurrentPage(state: OrdersStateModel): number {
    return state.graphQLQuery?.currentPage?.pageIndex ?? 0;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getPageSize(state: OrdersStateModel): number {
    return state.graphQLQuery?.currentPage?.pageSize ?? 0;
  }

  @Selector([ORDERS_STATE_TOKEN])
  static getSearchText(state: OrdersStateModel): string {
    return state.graphQLQuery.searchText || '';
  }

  @Action(LoadOrders)
  loadOrders(ctx: StateContext<OrdersStateModel>) {
    return this.ordersService.loadOrders(ctx.getState().graphQLFilters).pipe(
      tap(response => {
        ctx.patchState({
          orders: response.result.nodes,
          graphQLResponse: {
            result: response.result,
          },
        });
      })
    );
  }

  @Action(ChangePage)
  changePage(ctx: StateContext<OrdersStateModel>, action: ChangePage) {
    ctx.patchState(
      applyChangePageFilter<
        IntegratorQueryPartDefinitionOrdersArgs,
        PartDefinitionsOrderViewModel,
        PartDefinitionsOrderViewModelSortInput
      >(ctx.getState(), action.event, [
        {
          id: SortEnumType.Desc,
        },
      ])
    );

    return ctx.dispatch(new LoadOrders());
  }

  @Action(ApplyFilter)
  applyFilter(ctx: StateContext<OrdersStateModel>, action: ApplyFilter) {
    ctx.patchState(
      applyCommonSearchFilter<
        IntegratorQueryPartDefinitionOrdersArgs,
        PartDefinitionsOrderViewModel,
        PartDefinitionsOrderViewModelSortInput
      >(
        ctx.getState(),
        action.searchText,
        ['number'],
        [
          {
            id: SortEnumType.Desc,
          },
        ]
      )
    );

    return ctx.dispatch(new LoadOrders());
  }

  @Action(CreateOrder)
  createOrder(ctx: StateContext<OrdersStateModel>, action: CreateOrder) {
    return this.ordersService
      .createOrder({
        supplierId: action.supplierId,
        furnitureLineRequests: action.furnitureLineRequests,
        additionalNotes: action.additionalNotes,
      })
      .pipe(
        tap(() => {
          this.toastService.success('Zamówienie zostało utworzone pomyślnie');
          this.zone.run(() => {
            ctx.dispatch(new Navigate([FurnitureFormatsRoutePath.Orders]));
          });
          return ctx.dispatch(new LoadOrders());
        })
      );
  }
}
