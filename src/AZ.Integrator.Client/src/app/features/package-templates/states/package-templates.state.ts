import { Injectable, NgZone } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { tap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { PackageTemplatesStateModel } from './package-templates.state.model';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import { PackageTemplatesService } from '../services/package-templates.service';
import { LoadProductTags } from './package-templates.action';

const PACKAGE_TEMPLATES_STATE_TOKEN = new StateToken<PackageTemplatesStateModel>('packageTemplates');

@State<PackageTemplatesStateModel>({
  name: PACKAGE_TEMPLATES_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<string[]>(),
  },
})
@Injectable()
export class PackageTemplatesState {
  constructor(
    private packageTemplatesService: PackageTemplatesService,
    private zone: NgZone,
    private toastService: ToastrService
  ) {}

  @Selector([PACKAGE_TEMPLATES_STATE_TOKEN])
  static getProductTags(state: PackageTemplatesStateModel): string[] {
    return state.restQueryResponse.result;
  }

  @Action(LoadProductTags)
  loadInvoices(ctx: StateContext<PackageTemplatesStateModel>) {
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
}
