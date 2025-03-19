import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { TenantStateModel } from './tenant.state.model';
import { Tenant } from '../auth/models/tenant.model';
import { ChangeTenant } from './tenant.action';

export const TENANT_STATE_TOKEN = new StateToken<TenantStateModel>('tenant');

@State<TenantStateModel>({
  name: TENANT_STATE_TOKEN,
  defaults: {
    tenant: null,
  },
})
@Injectable()
export class TenantState {
  @Selector([TENANT_STATE_TOKEN])
  static getTenant(state: TenantStateModel): Tenant | null {
    return state?.tenant;
  }

  @Action(ChangeTenant)
  changeTenant(ctx: StateContext<TenantStateModel>, action: ChangeTenant) {
    ctx.patchState({
      tenant: action.tenant,
    });
  }
}
