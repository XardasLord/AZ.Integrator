import { inject, Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { tap } from 'rxjs';
import { FeatureFlagsStateModel } from './feature-flags.state.model.ts';
import { FeatureFlagsMapModel } from '../models/feature-flags-map.model';
import { Load } from './feature-flags.action';
import { FeatureFlagsService } from '../services/feature-flags.service';

export const SOURCE_SYSTEM_STATE_TOKEN = new StateToken<FeatureFlagsStateModel>('featureFlags');

@State<FeatureFlagsStateModel>({
  name: SOURCE_SYSTEM_STATE_TOKEN,
  defaults: {
    featureFlags: [],
  },
})
@Injectable()
export class FeatureFlagsState {
  private featureFlagsService = inject(FeatureFlagsService);

  @Selector([SOURCE_SYSTEM_STATE_TOKEN])
  static getFeatureFlags(state: FeatureFlagsStateModel): FeatureFlagsMapModel[] {
    return state.featureFlags;
  }

  @Action(Load)
  load(ctx: StateContext<FeatureFlagsStateModel>) {
    return this.featureFlagsService.load().pipe(
      tap(response => {
        ctx.patchState({
          featureFlags: response,
        });
      })
    );
  }
}
