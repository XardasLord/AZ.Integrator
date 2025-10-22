import { inject, Injectable, NgZone } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { FormatsStateModel } from './formats.state.model';
import { FurnitureFormatsService } from '../services/furniture-formats.service';
import {
  AddFurnitureDefinition,
  ApplyFilter,
  ChangePage,
  DeleteFurnitureDefinition,
  LoadFurnitureDefinitions,
  OpenFurnitureDefinitionDialog,
  UpdateFurnitureDefinition,
} from './formats.action';
import { GraphQLQueryVo } from '../../../../shared/graphql/graphql.query';
import { GraphQLResponse } from '../../../../shared/graphql/graphql.response';
import { applyChangePageFilter, applyCommonSearchFilter } from '../../../../shared/graphql/common-search-filter';
import {
  ConfirmationDialogComponent,
  ConfirmationDialogModel,
} from '../../../../shared/components/confirmation-dialog/confirmation-dialog.component';
import {
  FurnitureModelViewModel,
  FurnitureModelViewModelFilterInput,
  FurnitureModelViewModelSortInput,
  IntegratorQueryFurnitureModelsArgs,
  SortEnumType,
} from '../../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../../shared/helpers/name-of.helper';

const FORMATS_STATE_TOKEN = new StateToken<FormatsStateModel>('furnitureFormats');

@State<FormatsStateModel>({
  name: FORMATS_STATE_TOKEN,
  defaults: {
    furnitureDefinitions: [],
    graphQLQuery: new GraphQLQueryVo(),
    graphQLResponse: new GraphQLResponse<FurnitureModelViewModel[]>(),
    graphQLFilters: {},
  },
})
@Injectable()
export class FormatsState {
  private furnitureFormatsService = inject(FurnitureFormatsService);
  private zone = inject(NgZone);
  private dialog = inject(MatDialog);
  private toastService = inject(ToastrService);

  private dialogRef?: MatDialogRef<any>;

  @Selector([FORMATS_STATE_TOKEN])
  static getFurnitureDefinitions(state: FormatsStateModel): FurnitureModelViewModel[] {
    return state.furnitureDefinitions;
  }

  @Selector([FORMATS_STATE_TOKEN])
  static getTotalCount(state: FormatsStateModel): number {
    return state.graphQLResponse?.result?.totalCount ?? 0;
  }

  @Selector([FORMATS_STATE_TOKEN])
  static getCurrentPage(state: FormatsStateModel): number {
    return state.graphQLQuery?.currentPage?.pageIndex ?? 0;
  }

  @Selector([FORMATS_STATE_TOKEN])
  static getPageSize(state: FormatsStateModel): number {
    return state.graphQLQuery?.currentPage?.pageSize ?? 0;
  }

  @Selector([FORMATS_STATE_TOKEN])
  static getSearchText(state: FormatsStateModel): string {
    return state.graphQLQuery.searchText || '';
  }

  @Action(LoadFurnitureDefinitions)
  loadFurnitureDefinitions(ctx: StateContext<FormatsStateModel>) {
    return this.furnitureFormatsService.loadFurnitureDefinitions(ctx.getState().graphQLFilters).pipe(
      tap(response => {
        ctx.patchState({
          furnitureDefinitions: response.result.nodes,
          graphQLResponse: {
            result: response.result,
          },
        });
      })
    );
  }

  @Action(ChangePage)
  changePage(ctx: StateContext<FormatsStateModel>, action: ChangePage) {
    ctx.patchState(
      applyChangePageFilter<
        IntegratorQueryFurnitureModelsArgs,
        FurnitureModelViewModel,
        FurnitureModelViewModelSortInput
      >(ctx.getState(), action.event, [
        {
          furnitureCode: SortEnumType.Asc,
        },
      ])
    );

    return ctx.dispatch(new LoadFurnitureDefinitions());
  }

  @Action(ApplyFilter)
  applyFilter(ctx: StateContext<FormatsStateModel>, action: ApplyFilter): Observable<void> {
    ctx.patchState(
      applyCommonSearchFilter<
        IntegratorQueryFurnitureModelsArgs,
        FurnitureModelViewModel,
        FurnitureModelViewModelSortInput
      >(
        ctx.getState(),
        action.searchPhrase,
        [nameof<FurnitureModelViewModelFilterInput>('furnitureCode')],
        [
          {
            furnitureCode: SortEnumType.Asc,
          },
        ]
      )
    );

    return ctx.dispatch(new LoadFurnitureDefinitions());
  }

  @Action(AddFurnitureDefinition)
  addFurnitureDefinition(ctx: StateContext<FormatsStateModel>, action: AddFurnitureDefinition) {
    return this.furnitureFormatsService.addFurnitureDefinition(action.command).pipe(
      tap(() => {
        this.zone.run(() => this.toastService.success('Definicja mebla została zapisana', 'Definicja mebla'));
        if (this.dialogRef) {
          this.dialogRef.close();
        }

        ctx.dispatch(new LoadFurnitureDefinitions());
      }),
      catchError(error => {
        this.zone.run(() => this.toastService.error('Błąd podczas zapisu definicji mebla', 'Definicja mebla'));
        return throwError(() => error);
      })
    );
  }

  @Action(UpdateFurnitureDefinition)
  updateFurnitureDefinition(ctx: StateContext<FormatsStateModel>, action: UpdateFurnitureDefinition) {
    return this.furnitureFormatsService.updateFurnitureDefinition(action.command).pipe(
      tap(() => {
        this.zone.run(() => this.toastService.success('Definicja mebla została zapisana', 'Definicja mebla'));
        if (this.dialogRef) {
          this.dialogRef.close();
        }

        ctx.dispatch(new LoadFurnitureDefinitions());
      }),
      catchError(error => {
        this.zone.run(() => this.toastService.error('Błąd podczas zapisu definicji mebla', 'Definicja mebla'));
        return throwError(() => error);
      })
    );
  }

  @Action(DeleteFurnitureDefinition)
  deleteFurnitureDefinition(ctx: StateContext<FormatsStateModel>, action: DeleteFurnitureDefinition) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: new ConfirmationDialogModel(
        'Usuwanie definicji mebla',
        `Czy na pewno chcesz usunąć definicję mebla ${action.furnitureCode}?`
      ),
    });

    return dialogRef.afterClosed().pipe(
      tap(result => {
        if (!result) {
          return;
        }

        this.furnitureFormatsService.deleteFurnitureDefinition(action.furnitureCode).subscribe({
          next: () => {
            this.zone.run(() => this.toastService.success('Definicja mebla została usunięta', 'Definicja mebla'));
            ctx.dispatch(new LoadFurnitureDefinitions());
          },
          error: () => {
            this.zone.run(() => this.toastService.error('Błąd podczas usuwania definicji mebla', 'Definicja mebla'));
          },
        });
      })
    );
  }

  @Action(OpenFurnitureDefinitionDialog)
  openFurnitureDefinitionDialog(ctx: StateContext<FormatsStateModel>, action: OpenFurnitureDefinitionDialog) {
    // To be implemented with dialog component
  }
}
