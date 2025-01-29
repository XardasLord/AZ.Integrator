import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Observable, tap } from 'rxjs';
import { StocksStateModel } from './stocks.state.model';
import { ApplyFilter, ChangePage, LoadStocks } from './stocks.action';
import { IntegratorQueryStocksArgs, StockViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { RestQueryHelper } from '../../../shared/models/pagination/rest.helper';
import { GraphQLQueryVo } from '../../../shared/graphql/graphql.query';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StocksService } from '../services/stocks.service';

const STOCKS_STATE_TOKEN = new StateToken<StocksStateModel>('stocks');

@State<StocksStateModel>({
  name: STOCKS_STATE_TOKEN,
  defaults: {
    graphqlQuery: new GraphQLQueryVo(),
    graphqlQueryResponse: new GraphQLResponseWithoutPaginationVo<StockViewModel[]>(),
    stocks: [],
  },
})
@Injectable()
export class StocksState {
  constructor(private stocksService: StocksService) {}

  @Selector([STOCKS_STATE_TOKEN])
  static getStocks(state: StocksStateModel): StockViewModel[] {
    return state.stocks;
  }

  @Selector([STOCKS_STATE_TOKEN])
  static getStocksCount(state: StocksStateModel): number {
    return state.graphqlQueryResponse.result.length;
  }

  @Selector([STOCKS_STATE_TOKEN])
  static getPageSize(state: StocksStateModel): number {
    return state.graphqlQuery.currentPage.pageSize;
  }

  @Selector([STOCKS_STATE_TOKEN])
  static getSearchText(state: StocksStateModel): string {
    return state.graphqlQuery.searchText!;
  }

  @Action(LoadStocks)
  loadStocks(ctx: StateContext<StocksStateModel>) {
    const filters: IntegratorQueryStocksArgs = {};

    filters.where = {
      packageCode: {
        contains: ctx.getState().graphqlQuery.searchText,
      },
    };

    return this.stocksService.getStocks(filters).pipe(
      tap(response => {
        ctx.patchState({
          stocks: response.result,
        });
      })
    );
  }

  @Action(ChangePage)
  changePage(ctx: StateContext<StocksStateModel>, action: ChangePage) {
    const customQuery = new GraphQLQueryVo();
    customQuery.currentPage = action.event;

    ctx.patchState({
      graphqlQuery: customQuery,
    });

    return ctx.dispatch(new LoadStocks());
  }

  @Action(ApplyFilter)
  applyFilter(ctx: StateContext<StocksStateModel>, action: ApplyFilter): Observable<void> {
    const updatedQuery = { ...ctx.getState().graphqlQuery };
    updatedQuery.searchText = action.searchPhrase;
    updatedQuery.currentPage = RestQueryHelper.getInitialPageEvent();

    ctx.patchState({
      graphqlQuery: updatedQuery,
    });

    return ctx.dispatch(new LoadStocks());
  }
}
