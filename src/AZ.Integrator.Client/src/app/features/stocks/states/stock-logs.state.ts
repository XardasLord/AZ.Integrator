import { inject, Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Observable, tap } from 'rxjs';
import {
  IntegratorQueryBarcodeScannerLogsArgs,
  SortEnumType,
  StockLogViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLQueryVo } from '../../../shared/graphql/graphql.query';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StocksService } from '../services/stocks.service';
import { StockLogsStateModel } from './stock-logs.state.model';
import { ApplyFilters, LoadLogs } from './stock-logs.action';

const STOCK_LOGS_STATE_TOKEN = new StateToken<StockLogsStateModel>('stockLogs');

@State<StockLogsStateModel>({
  name: STOCK_LOGS_STATE_TOKEN,
  defaults: {
    graphqlQuery: new GraphQLQueryVo(),
    graphqlQueryResponse: new GraphQLResponseWithoutPaginationVo<StockLogViewModel[]>(),
    dateFilter: { from: new Date(new Date().setDate(new Date().getDate() - 30)), to: new Date() },
    logs: [],
  },
})
@Injectable()
export class StockLogsState {
  private stocksService = inject(StocksService);

  @Selector([STOCK_LOGS_STATE_TOKEN])
  static logs(state: StockLogsStateModel): StockLogViewModel[] {
    return state.logs;
  }

  @Selector([STOCK_LOGS_STATE_TOKEN])
  static searchText(state: StockLogsStateModel): string {
    return state.graphqlQuery.searchText!;
  }

  @Selector([STOCK_LOGS_STATE_TOKEN])
  static dateFilter(state: StockLogsStateModel): { from: Date; to: Date } {
    return state.dateFilter;
  }

  @Action(LoadLogs)
  loadLogs(ctx: StateContext<StockLogsStateModel>) {
    const state = ctx.getState();
    const filters: IntegratorQueryBarcodeScannerLogsArgs = {};

    filters.order = [
      {
        id: SortEnumType.Desc,
      },
    ];

    filters.where = {
      createdAt: {
        gte: state.dateFilter.from,
        lte: state.dateFilter.to,
      },
      ...(state.graphqlQuery.searchText && { createdBy: { contains: state.graphqlQuery.searchText } }),
    };

    return this.stocksService.getBarcodeScannerLogs(filters).pipe(
      tap(response => {
        ctx.patchState({
          logs: response.result,
        });
      })
    );
  }

  @Action(ApplyFilters)
  applyFilter(ctx: StateContext<StockLogsStateModel>, action: ApplyFilters): Observable<void> {
    const updatedQuery = { ...ctx.getState().graphqlQuery };
    updatedQuery.searchText = action.searchPhrase;

    ctx.patchState({
      graphqlQuery: updatedQuery,
      dateFilter: {
        from: action.startDate,
        to: action.endDate,
      },
    });

    return ctx.dispatch(new LoadLogs());
  }
}
