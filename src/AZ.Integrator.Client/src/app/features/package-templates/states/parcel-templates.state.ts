import { Injectable, NgZone } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ParcelTemplatesStateModel } from './parcel-templates.state.model';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import { ParcelTemplatesService } from '../services/parcel-templates.service';
import { ParcelTemplateDefinitionModalComponent } from '../pages/parcel-template-definition-modal/parcel-template-definition-modal.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ParcelTemplateDefinitionDataModel } from '../models/parcel-template-definition-data.model';
import { LoadProductTags, OpenPackageTemplateDefinitionModal, SavePackageTemplate } from './parcel-templates.action';

const PACKAGE_TEMPLATES_STATE_TOKEN = new StateToken<ParcelTemplatesStateModel>('packageTemplates');

@State<ParcelTemplatesStateModel>({
  name: PACKAGE_TEMPLATES_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<string[]>(),
  },
})
@Injectable()
export class ParcelTemplatesState {
  private dialogRef?: MatDialogRef<ParcelTemplateDefinitionModalComponent>;

  constructor(
    private packageTemplatesService: ParcelTemplatesService,
    private zone: NgZone,
    private dialog: MatDialog,
    private toastService: ToastrService
  ) {}

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getProductTags(state: ParcelTemplatesStateModel): string[] {
    return state.restQueryResponse.result;
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
          restQueryResponse: {
            result: response,
            totalCount: response.length,
          },
        });
      })
    );
  }

  @Action(OpenPackageTemplateDefinitionModal)
  openRegisterDpdShipmentModal(
    ctx: StateContext<ParcelTemplatesStateModel>,
    action: OpenPackageTemplateDefinitionModal
  ) {
    this.zone.run(() => {
      const data: ParcelTemplateDefinitionDataModel = {
        tag: action.tag,
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