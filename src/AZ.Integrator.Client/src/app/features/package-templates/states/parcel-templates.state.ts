import { inject, Injectable, NgZone } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Action, Selector, State, StateContext, StateToken, Store } from '@ngxs/store';
import { catchError, map, Observable, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { GetOfferSignaturesResponse, ParcelTemplatesStateModel } from './parcel-templates.state.model';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import { ParcelTemplatesService } from '../services/parcel-templates.service';
import { ParcelTemplateDefinitionModalComponent } from '../pages/parcel-template-definition-modal/parcel-template-definition-modal.component';
import { ParcelTemplateDefinitionDataModel } from '../models/parcel-template-definition-data.model';
import {
  ApplyFilter,
  ChangePage,
  LoadProductTags,
  OpenPackageTemplateDefinitionModal,
  SavePackageTemplate,
} from './parcel-templates.action';
import { GetTagParcelTemplatesGQL } from '../../../shared/graphql/queries/get-tag-parcel-templates.graphql.query';
import { IntegratorQueryTagParcelTemplatesArgs } from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLHelper } from '../../../shared/graphql/graphql.helper';
import { RestQueryHelper } from '../../../shared/models/pagination/rest.helper';
import { TenantState } from '../../../shared/states/tenant.state';

const PACKAGE_TEMPLATES_STATE_TOKEN = new StateToken<ParcelTemplatesStateModel>('packageTemplates');

@State<ParcelTemplatesStateModel>({
  name: PACKAGE_TEMPLATES_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<GetOfferSignaturesResponse>(),
    signatures: [],
  },
})
@Injectable()
export class ParcelTemplatesState {
  private store = inject(Store);
  private packageTemplatesService = inject(ParcelTemplatesService);
  private getTagParcelTemplatesGql = inject(GetTagParcelTemplatesGQL);
  private zone = inject(NgZone);
  private dialog = inject(MatDialog);
  private toastService = inject(ToastrService);

  private dialogRef?: MatDialogRef<ParcelTemplateDefinitionModalComponent>;

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getProductTags(state: ParcelTemplatesStateModel): string[] {
    return state.signatures;
  }

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getProductTagsCount(state: ParcelTemplatesStateModel): number {
    return state.restQueryResponse.totalCount;
  }

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getCurrentPage(state: ParcelTemplatesStateModel): number {
    return state.restQuery.currentPage.pageIndex;
  }

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getPageSize(state: ParcelTemplatesStateModel): number {
    return state.restQuery.currentPage.pageSize;
  }

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getSearchText(state: ParcelTemplatesStateModel): string {
    return state.restQuery.searchText;
  }

  @Action(LoadProductTags)
  loadInvoices(ctx: StateContext<ParcelTemplatesStateModel>) {
    const state = ctx.getState();

    return this.packageTemplatesService.loadProductTags(state.restQuery.currentPage, state.restQuery.searchText).pipe(
      tap(response => {
        ctx.patchState({
          signatures: response.signatures,
          restQueryResponse: {
            result: response,
            totalCount: response.totalCount,
          },
        });
      })
    );
  }

  @Action(ChangePage)
  changePage(ctx: StateContext<ParcelTemplatesStateModel>, action: ChangePage) {
    const customQuery = new RestQueryVo();
    customQuery.currentPage = action.event;

    ctx.patchState({
      restQuery: customQuery,
    });

    return ctx.dispatch(new LoadProductTags());
  }

  @Action(ApplyFilter)
  applyFilter(ctx: StateContext<ParcelTemplatesStateModel>, action: ApplyFilter): Observable<void> {
    const updatedQuery = { ...ctx.getState().restQuery };
    updatedQuery.searchText = action.searchPhrase;
    updatedQuery.currentPage = RestQueryHelper.getInitialPageEvent();

    ctx.patchState({
      restQuery: updatedQuery,
    });

    return ctx.dispatch(new LoadProductTags());
  }

  @Action(OpenPackageTemplateDefinitionModal)
  openPackageTemplateDefinitionModal(
    ctx: StateContext<ParcelTemplatesStateModel>,
    action: OpenPackageTemplateDefinitionModal
  ) {
    this.zone.run(() => {
      const query: IntegratorQueryTagParcelTemplatesArgs = {
        where: {
          tag: {
            eq: action.tag,
          },
          tenantId: {
            eq: this.store.selectSnapshot(TenantState.getTenant)?.tenantId,
          },
        },
      };

      this.getTagParcelTemplatesGql
        .watch(query, GraphQLHelper.defaultGraphQLWatchQueryOptions)
        .valueChanges.pipe(map(x => x.data.result))
        .subscribe(results => {
          const data: ParcelTemplateDefinitionDataModel = {
            tag: action.tag,
            template: results[0] ?? null,
          };

          this.dialogRef = this.dialog.open<ParcelTemplateDefinitionModalComponent, ParcelTemplateDefinitionDataModel>(
            ParcelTemplateDefinitionModalComponent,
            {
              data: <ParcelTemplateDefinitionDataModel>data,
              width: '50%',
              height: '70%',
            }
          );
        });
    });
  }

  @Action(SavePackageTemplate)
  savePackageTemplate(ctx: StateContext<ParcelTemplatesStateModel>, action: SavePackageTemplate) {
    return this.packageTemplatesService.saveTemplate(action.command).pipe(
      tap(() => {
        this.zone.run(() => this.toastService.success('Szablon przesyłki został zapisany', 'Szablon przesyłki'));
        this.dialogRef?.close();

        ctx.dispatch(new LoadProductTags());
      }),
      catchError(error => {
        this.zone.run(() => this.toastService.error('Błąd podczas zapisu szablonu przesyłki', 'Szablon przesyłki'));
        return throwError(error);
      })
    );
  }
}
