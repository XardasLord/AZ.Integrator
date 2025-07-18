import { inject, Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { patch, updateItem } from '@ngxs/store/operators';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { StocksStateModel } from './stocks.state.model';
import {
  AddStockGroup,
  ApplyFilter,
  ChangeGroup,
  ChangePage,
  LoadStockGroups,
  LoadStocks,
  UpdateStockGroup,
} from './stocks.action';
import {
  IntegratorQueryStockGroupsArgs,
  IntegratorQueryStocksArgs,
  SortEnumType,
  StockGroupViewModel,
  StockViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';
import { RestQueryHelper } from '../../../shared/models/pagination/rest.helper';
import { GraphQLQueryVo } from '../../../shared/graphql/graphql.query';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StocksService } from '../services/stocks.service';
import { StockGroupsService } from '../services/stock-groups.service';

const STOCKS_STATE_TOKEN = new StateToken<StocksStateModel>('stocks');

@State<StocksStateModel>({
  name: STOCKS_STATE_TOKEN,
  defaults: {
    graphqlQuery: new GraphQLQueryVo(),
    graphqlQueryResponse: new GraphQLResponseWithoutPaginationVo<StockViewModel[]>(),
    stocks: [],
    stockGroups: [],
  },
})
@Injectable()
export class StocksState {
  private stocksService = inject(StocksService);
  private stockGroupsService = inject(StockGroupsService);
  private toastService = inject(ToastrService);

  @Selector([STOCKS_STATE_TOKEN])
  static stocks(state: StocksStateModel): StockViewModel[] {
    return state.stocks;
  }

  @Selector()
  static groupedStocks(state: StocksStateModel): StockGroupViewModel[] {
    const ungroupedStocks = state.stocks.filter(s => !s.groupId);

    let allGroups: StockGroupViewModel[] = [];

    if (ungroupedStocks.length) {
      allGroups.push({
        __typename: 'StockGroupViewModel',
        id: 0,
        name: 'Bez grupy',
        description: '',
        stocks: ungroupedStocks.map(s => s),
      });
    }

    allGroups = [
      ...allGroups,
      ...state.stockGroups.map(g => ({
        ...g,
        stocks: state.stocks.filter(s => s.groupId === g.id),
      })),
    ];

    return allGroups;
  }

  @Selector([STOCKS_STATE_TOKEN])
  static stocksCount(state: StocksStateModel): number {
    return state.graphqlQueryResponse.result.length;
  }

  @Selector([STOCKS_STATE_TOKEN])
  static pageSize(state: StocksStateModel): number {
    return state.graphqlQuery.currentPage.pageSize;
  }

  @Selector([STOCKS_STATE_TOKEN])
  static searchText(state: StocksStateModel): string {
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

    filters.order = [
      {
        quantity: SortEnumType.Desc,
      },
    ];

    return this.stocksService.getStocks(filters).pipe(
      tap(response => {
        ctx.patchState({
          stocks: response.result,
        });
      })
    );
  }

  @Action(LoadStockGroups)
  loadStockGroups(ctx: StateContext<StocksStateModel>) {
    const filters: IntegratorQueryStockGroupsArgs = {};

    filters.order = [
      {
        name: SortEnumType.Asc,
      },
    ];

    return this.stockGroupsService.getStockGroups(filters).pipe(
      tap(response => {
        ctx.patchState({
          stockGroups: response.result,
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

  @Action(AddStockGroup)
  addStockGroup(ctx: StateContext<StocksStateModel>, action: AddStockGroup) {
    return this.stockGroupsService.add(action.name, action.description).pipe(
      tap(() => {
        ctx.dispatch(new LoadStockGroups());

        this.toastService.success(`Grupa '${action.name}' została poprawnie dodana`);
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleException(error);
      })
    );
  }

  @Action(UpdateStockGroup)
  updateStockGroup(ctx: StateContext<StocksStateModel>, action: UpdateStockGroup) {
    return this.stockGroupsService.update(action.groupId, action.name, action.description).pipe(
      tap(() => {
        ctx.setState(
          patch({
            stockGroups: updateItem<StockGroupViewModel>(
              group => group.id === action.groupId,
              patch({ name: action.name, description: action.description })
            ),
          })
        );

        this.toastService.success(`Grupa '${action.name}' została poprawnie edytowana`);
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleException(error);
      })
    );
  }

  @Action(ChangeGroup)
  changeGroup(ctx: StateContext<StocksStateModel>, action: ChangeGroup) {
    return this.stocksService.changeGroup(action.packageCode, action.newGroupId).pipe(
      tap(() => {
        ctx.setState(
          patch({
            stocks: updateItem<StockViewModel>(
              stock => stock.packageCode === action.packageCode,
              patch({ groupId: action.newGroupId })
            ),
          })
        );

        this.toastService.success(`Kod '${action.packageCode}' postał poprawnie przypisany do nowej grupy`);
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleException(error);
      })
    );
  }

  private handleException(error: HttpErrorResponse) {
    this.toastService.error(`Wystąpił błąd podczas wysyłania żądania do serwera - ${error.error.Message}`);
    return throwError(() => new Error(error.message));
  }
}
