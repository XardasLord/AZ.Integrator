import { inject, Injectable, NgZone } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { SuppliersStateModel } from './suppliers.state.model';
import { SuppliersService } from '../services/suppliers.service';
import {
  AddSupplier,
  ApplyFilter,
  ChangePage,
  DeleteSupplier,
  LoadSuppliers,
  UpdateSupplier,
} from './suppliers.action';
import { GraphQLQueryVo } from '../../../../shared/graphql/graphql.query';
import { GraphQLResponse } from '../../../../shared/graphql/graphql.response';
import { applyChangePageFilter, applyCommonSearchFilter } from '../../../../shared/graphql/common-search-filter';
import {
  ConfirmationDialogComponent,
  ConfirmationDialogModel,
} from '../../../../shared/components/confirmation-dialog/confirmation-dialog.component';
import {
  IntegratorQuerySuppliersArgs,
  SortEnumType,
  SupplierViewModel,
  SupplierViewModelFilterInput,
  SupplierViewModelSortInput,
} from '../../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../../shared/helpers/name-of.helper';

const SUPPLIERS_STATE_TOKEN = new StateToken<SuppliersStateModel>('suppliers');

@State<SuppliersStateModel>({
  name: SUPPLIERS_STATE_TOKEN,
  defaults: {
    suppliers: [],
    graphQLQuery: new GraphQLQueryVo(),
    graphQLResponse: new GraphQLResponse<SupplierViewModel[]>(),
    graphQLFilters: {},
  },
})
@Injectable()
export class SuppliersState {
  private suppliersService = inject(SuppliersService);
  private zone = inject(NgZone);
  private dialog = inject(MatDialog);
  private toastService = inject(ToastrService);

  @Selector([SUPPLIERS_STATE_TOKEN])
  static getSuppliers(state: SuppliersStateModel): SupplierViewModel[] {
    return state.suppliers;
  }

  @Selector([SUPPLIERS_STATE_TOKEN])
  static getTotalCount(state: SuppliersStateModel): number {
    return state.graphQLResponse?.result?.totalCount ?? 0;
  }

  @Selector([SUPPLIERS_STATE_TOKEN])
  static getCurrentPage(state: SuppliersStateModel): number {
    return state.graphQLQuery?.currentPage?.pageIndex ?? 0;
  }

  @Selector([SUPPLIERS_STATE_TOKEN])
  static getPageSize(state: SuppliersStateModel): number {
    return state.graphQLQuery?.currentPage?.pageSize ?? 0;
  }

  @Selector([SUPPLIERS_STATE_TOKEN])
  static getSearchText(state: SuppliersStateModel): string {
    return state.graphQLQuery.searchText || '';
  }

  @Action(LoadSuppliers)
  loadSuppliers(ctx: StateContext<SuppliersStateModel>) {
    return this.suppliersService.loadSuppliers(ctx.getState().graphQLFilters).pipe(
      tap(response => {
        ctx.patchState({
          suppliers: response.result.nodes,
          graphQLResponse: {
            result: response.result,
          },
        });
      })
    );
  }

  @Action(ChangePage)
  changePage(ctx: StateContext<SuppliersStateModel>, action: ChangePage) {
    ctx.patchState(
      applyChangePageFilter<IntegratorQuerySuppliersArgs, SupplierViewModel, SupplierViewModelSortInput>(
        ctx.getState(),
        action.event,
        [
          {
            name: SortEnumType.Asc,
          },
        ]
      )
    );

    return ctx.dispatch(new LoadSuppliers());
  }

  @Action(ApplyFilter)
  applyFilter(ctx: StateContext<SuppliersStateModel>, action: ApplyFilter): Observable<void> {
    ctx.patchState(
      applyCommonSearchFilter<IntegratorQuerySuppliersArgs, SupplierViewModel, SupplierViewModelSortInput>(
        ctx.getState(),
        action.searchPhrase,
        [nameof<SupplierViewModelFilterInput>('name')],
        [
          {
            name: SortEnumType.Asc,
          },
        ]
      )
    );

    return ctx.dispatch(new LoadSuppliers());
  }

  @Action(AddSupplier)
  addSupplier(ctx: StateContext<SuppliersStateModel>, action: AddSupplier) {
    return this.suppliersService.addSupplier(action.command).pipe(
      tap(() => {
        this.zone.run(() => this.toastService.success('Dostawca został zapisany', 'Dostawca'));
        ctx.dispatch(new LoadSuppliers());
      }),
      catchError(error => {
        this.zone.run(() => this.toastService.error('Błąd podczas zapisu dostawcy', 'Dostawca'));
        return throwError(() => error);
      })
    );
  }

  @Action(UpdateSupplier)
  updateSupplier(ctx: StateContext<SuppliersStateModel>, action: UpdateSupplier) {
    return this.suppliersService.updateSupplier(action.command).pipe(
      tap(() => {
        this.zone.run(() => this.toastService.success('Dostawca został zaktualizowany', 'Dostawca'));
        ctx.dispatch(new LoadSuppliers());
      }),
      catchError(error => {
        this.zone.run(() => this.toastService.error('Błąd podczas aktualizacji dostawcy', 'Dostawca'));
        return throwError(() => error);
      })
    );
  }

  @Action(DeleteSupplier)
  deleteSupplier(ctx: StateContext<SuppliersStateModel>, action: DeleteSupplier) {
    const supplier = ctx.getState().suppliers.find(s => s.id === action.supplierId);
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: new ConfirmationDialogModel('Usuwanie dostawcy', `Czy na pewno chcesz usunąć dostawcę ${supplier?.name}?`),
    });

    return dialogRef.afterClosed().pipe(
      tap(result => {
        if (!result) {
          return;
        }

        this.suppliersService.deleteSupplier(action.supplierId).subscribe({
          next: () => {
            this.zone.run(() => this.toastService.success('Dostawca został usunięty', 'Dostawca'));
            ctx.dispatch(new LoadSuppliers());
          },
          error: () => {
            this.zone.run(() => this.toastService.error('Błąd podczas usuwania dostawcy', 'Dostawca'));
          },
        });
      })
    );
  }
}
