import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { SourceSystemStateModel } from './source-system.state.model';
import { SourceSystem } from '../auth/models/source-system.model';
import { ChangeSourceSystem } from './source-system.action';

export const SOURCE_SYSTEM_STATE_TOKEN = new StateToken<SourceSystemStateModel>('sourceSystem');

@State<SourceSystemStateModel>({
  name: SOURCE_SYSTEM_STATE_TOKEN,
  defaults: {
    sourceSystem: null,
  },
})
@Injectable()
export class SourceSystemState {
  @Selector([SOURCE_SYSTEM_STATE_TOKEN])
  static getSourceSystem(state: SourceSystemStateModel): SourceSystem | null {
    return state?.sourceSystem;
  }

  @Action(ChangeSourceSystem)
  changeSourceSystem(ctx: StateContext<SourceSystemStateModel>, action: ChangeSourceSystem) {
    ctx.patchState({
      sourceSystem: action.sourceSystem,
    });
  }
}
