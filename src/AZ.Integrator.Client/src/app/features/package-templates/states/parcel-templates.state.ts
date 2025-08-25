import { inject, Injectable, NgZone } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ParcelTemplatesStateModel } from './parcel-templates.state.model';
import { ParcelTemplatesService } from '../services/parcel-templates.service';
import { PackageTemplateDefinitionFormDialogComponent } from '../components/package-template-definition-form-dialog/package-template-definition-form-dialog.component';
import { ApplyFilter, ChangePage, LoadTemplates, SavePackageTemplate } from './parcel-templates.action';
import {
  IntegratorQueryTagParcelTemplatesArgs,
  SortEnumType,
  TagParcelTemplateViewModel,
  TagParcelTemplateViewModelFilterInput,
  TagParcelTemplateViewModelSortInput,
} from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLQueryVo } from '../../../shared/graphql/graphql.query';
import { GraphQLResponse } from '../../../shared/graphql/graphql.response';
import { applyChangePageFilter, applyCommonSearchFilter } from '../../../shared/graphql/common-search-filter';
import { nameof } from 'src/app/shared/helpers/name-of.helper';

const PACKAGE_TEMPLATES_STATE_TOKEN = new StateToken<ParcelTemplatesStateModel>('packageTemplates');

@State<ParcelTemplatesStateModel>({
  name: PACKAGE_TEMPLATES_STATE_TOKEN,
  defaults: {
    templates: [],
    graphQLQuery: new GraphQLQueryVo(),
    graphQLResponse: new GraphQLResponse<TagParcelTemplateViewModel[]>(),
    graphQLFilters: {},
  },
})
@Injectable()
export class ParcelTemplatesState {
  private packageTemplatesService = inject(ParcelTemplatesService);
  private zone = inject(NgZone);
  private dialog = inject(MatDialog);
  private toastService = inject(ToastrService);

  private dialogRef?: MatDialogRef<PackageTemplateDefinitionFormDialogComponent>;

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getTemplates(state: ParcelTemplatesStateModel): TagParcelTemplateViewModel[] {
    return state.templates;
  }

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getTemplatesCount(state: ParcelTemplatesStateModel): number {
    return state.graphQLResponse?.result?.totalCount ?? 0;
  }

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getCurrentPage(state: ParcelTemplatesStateModel): number {
    return state.graphQLQuery?.currentPage?.pageIndex ?? 0;
  }

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getPageSize(state: ParcelTemplatesStateModel): number {
    return state.graphQLQuery?.currentPage?.pageSize ?? 0;
  }

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getSearchText(state: ParcelTemplatesStateModel): string {
    return state.graphQLQuery.searchText || '';
  }

  @Action(LoadTemplates)
  loadTemplates(ctx: StateContext<ParcelTemplatesStateModel>) {
    return this.packageTemplatesService.loadTemplates(ctx.getState().graphQLFilters).pipe(
      tap(response => {
        ctx.patchState({
          templates: response.result.nodes,
          graphQLResponse: {
            result: response.result,
          },
        });
      })
    );
  }

  @Action(ChangePage)
  changePage(ctx: StateContext<ParcelTemplatesStateModel>, action: ChangePage) {
    ctx.patchState(
      applyChangePageFilter<
        IntegratorQueryTagParcelTemplatesArgs,
        TagParcelTemplateViewModel,
        TagParcelTemplateViewModelSortInput
      >(ctx.getState(), action.event, [
        {
          tag: SortEnumType.Asc,
        },
      ])
    );

    return ctx.dispatch(new LoadTemplates());
  }

  @Action(ApplyFilter)
  applyFilter(ctx: StateContext<ParcelTemplatesStateModel>, action: ApplyFilter): Observable<void> {
    ctx.patchState(
      applyCommonSearchFilter<
        IntegratorQueryTagParcelTemplatesArgs,
        TagParcelTemplateViewModel,
        TagParcelTemplateViewModelSortInput
      >(
        ctx.getState(),
        action.searchPhrase,
        [nameof<TagParcelTemplateViewModelFilterInput>('tag')],
        [
          {
            tag: SortEnumType.Asc,
          },
        ]
      )
    );

    return ctx.dispatch(new LoadTemplates());
  }

  @Action(SavePackageTemplate)
  savePackageTemplate(ctx: StateContext<ParcelTemplatesStateModel>, action: SavePackageTemplate) {
    return this.packageTemplatesService.saveTemplate(action.command).pipe(
      tap(() => {
        this.zone.run(() => this.toastService.success('Szablon przesyłki został zapisany', 'Szablon przesyłki'));
        this.dialogRef?.close();

        ctx.dispatch(new LoadTemplates());
      }),
      catchError(error => {
        this.zone.run(() => this.toastService.error('Błąd podczas zapisu szablonu przesyłki', 'Szablon przesyłki'));
        return throwError(error);
      })
    );
  }
}
