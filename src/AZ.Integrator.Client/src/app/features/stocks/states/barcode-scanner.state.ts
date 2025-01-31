import { inject, Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken, Store } from '@ngxs/store';
import { append, patch } from '@ngxs/store/operators';
import { catchError, tap, throwError } from 'rxjs';
import { BarcodeScannerStateModel } from './barcode-scanner.state.model';
import {
  IntegratorQueryBarcodeScannerLogsArgs,
  StockLogViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StocksService } from '../services/stocks.service';
import { DecreaseStock, IncreaseStock, LoadLogs } from './barcode-scanner.action';
import { AuthState } from '../../../shared/states/auth.state';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

const BARCODE_SCANNER_STATE_TOKEN = new StateToken<BarcodeScannerStateModel>('barcodeScanner');

@State<BarcodeScannerStateModel>({
  name: BARCODE_SCANNER_STATE_TOKEN,
  defaults: {
    graphqlQueryResponse: new GraphQLResponseWithoutPaginationVo<StockLogViewModel[]>(),
    logs: [],
  },
})
@Injectable()
export class BarcodeScannerState {
  private store = inject(Store);
  private stocksService = inject(StocksService);
  private toastService = inject(ToastrService);

  @Selector([BARCODE_SCANNER_STATE_TOKEN])
  static logs(state: BarcodeScannerStateModel): StockLogViewModel[] {
    return state.logs;
  }

  @Action(LoadLogs)
  loadLogs(ctx: StateContext<BarcodeScannerStateModel>) {
    const filters: IntegratorQueryBarcodeScannerLogsArgs = {};

    filters.where = {
      createdBy: { eq: this.store.selectSnapshot(AuthState.getProfile)?.username },
    };

    return this.stocksService.getBarcodeScannerLogs(filters).pipe(
      tap(response => {
        ctx.patchState({
          logs: response.result,
        });
      })
    );
  }

  @Action(IncreaseStock)
  increaseStock(ctx: StateContext<BarcodeScannerStateModel>, action: IncreaseStock) {
    return this.stocksService.updateStockQuantity(action.barcode, action.changeQuantity).pipe(
      tap(() => {
        ctx.setState(
          patch<BarcodeScannerStateModel>({
            logs: append<StockLogViewModel>([
              {
                id: ctx.getState().logs.length + 1,
                changeQuantity: action.changeQuantity,
                packageCode: action.barcode,
                createdBy: this.store.selectSnapshot(AuthState.getProfile)?.username,
                createdAt: new Date(),
              },
            ]),
          })
        );

        this.toastService.success(`Stan magazynowy dla kodu ${action.barcode} został poprawnie dodany`);
      }),
      catchError((error: HttpErrorResponse) => {
        this.toastService.error(`Wystąpił błąd podczas wysyłania żądania do serwera - ${error.error.Message}`);
        return throwError(() => new Error(error.message));
      })
    );
  }

  @Action(DecreaseStock)
  decreaseStock(ctx: StateContext<BarcodeScannerStateModel>, action: DecreaseStock) {
    return this.stocksService.updateStockQuantity(action.barcode, action.changeQuantity).pipe(
      tap(() => {
        ctx.setState(
          patch<BarcodeScannerStateModel>({
            logs: append<StockLogViewModel>([
              {
                id: ctx.getState().logs.length + 1,
                changeQuantity: action.changeQuantity,
                packageCode: action.barcode,
                createdBy: this.store.selectSnapshot(AuthState.getProfile)?.username,
                createdAt: new Date(),
              },
            ]),
          })
        );

        this.toastService.success(`Stan magazynowy dla kodu ${action.barcode} został poprawnie odjęty`);
      }),
      catchError((error: HttpErrorResponse) => {
        console.warn(error);
        this.toastService.error(`Wystąpił błąd podczas wysyłania żądania do serwera - ${error.error.Message}`);
        return throwError(() => new Error(error.message));
      })
    );
  }
}
