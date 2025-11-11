import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RemoteServiceBase } from '../../services/remote-service.base';
import { FeatureFlagsMapModel } from '../models/feature-flags-map.model';
import { Store } from '@ngxs/store';
import { FeatureFlagsState } from '../store/feature-flags.state.ts';

@Injectable()
export class FeatureFlagsService extends RemoteServiceBase {
  private store = inject(Store);

  constructor() {
    const httpClient = inject(HttpClient);

    super(httpClient);
  }

  public load(): Observable<FeatureFlagsMapModel[]> {
    const url = `${this.apiEndpoint}/me/feature-flags`;

    return this.httpClient.get<FeatureFlagsMapModel[]>(url);
  }

  public isEnabled(featureFlag: string): boolean {
    const featureFlags = this.store.selectSnapshot(FeatureFlagsState.getFeatureFlags);

    console.log(featureFlag, featureFlags);

    return featureFlags.some(flagsMap => flagsMap[featureFlag]);
  }
}
