import { Component, inject, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApplyFilter, LoadOrders } from '../../states/orders.action';
import { OrdersState } from '../../states/orders-state.service';
import { DebounceDirective } from '../../../../shared/directives/debounce.directive';
import { AsyncPipe } from '@angular/common';
import { MaterialModule } from '../../../../shared/modules/material.module';
import {
  AuthorizationProvider,
  SourceSystem,
  SourceSystemGroup,
} from '../../../../shared/auth/models/source-system.model';
import { ChangeSourceSystem } from '../../../../shared/states/source-system.action';
import { SourceSystemState } from '../../../../shared/states/source-system.state';
import { AuthRoles } from '../../../../shared/auth/models/auth.roles';
import { AuthRoleAllowDirective } from '../../../../shared/auth/directives/auth-role-allow.directive';
import { IntegrationsState } from '../../../integrations/states/integrations.state';
import { LoadIntegrations } from '../../../integrations/states/integrations.action';
import { IntegrationToSourceSystemHelper } from '../../helpers/integration-to-source-system.helper';
import { IntegrationsRoutePath } from '../../../../core/modules/app-routing.module';
import { Navigate } from '@ngxs/router-plugin';

@Component({
  selector: 'app-orders-filters',
  templateUrl: './orders-filters.component.html',
  styleUrls: ['./orders-filters.component.scss'],
  imports: [MaterialModule, DebounceDirective, AsyncPipe, AuthRoleAllowDirective],
})
export class OrdersFiltersComponent implements OnInit {
  private store = inject(Store);

  searchText$: Observable<string> = this.store.select(OrdersState.getSearchText);
  selectedStore$ = this.store.select(SourceSystemState.getSourceSystem);
  sourceSystemGroups$: Observable<SourceSystemGroup[]> = this.store
    .select(IntegrationsState.activeMarketplaceIntegrations)
    .pipe(map(integrations => IntegrationToSourceSystemHelper.convertToSourceSystemGroups(integrations)));
  hasActiveMarketplaceIntegrations$: Observable<boolean> = this.store
    .select(IntegrationsState.activeMarketplaceIntegrations)
    .pipe(map(integrations => integrations.length > 0));

  ngOnInit() {
    this.store.dispatch(new LoadIntegrations());
  }

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilter(searchText));
  }

  onStoreSelected(sourceSystem: SourceSystem) {
    this.store.dispatch(new ChangeSourceSystem(sourceSystem));
    this.store.dispatch(new LoadOrders());
  }

  iconFor(platform: AuthorizationProvider) {
    switch (platform) {
      case AuthorizationProvider.Allegro:
        return 'storefront';
      case AuthorizationProvider.Erli:
        return 'storefront';
      case AuthorizationProvider.Shopify:
        return 'storefront';
    }
  }

  compareStores(store1: SourceSystem, store2: SourceSystem): boolean {
    return store1 && store2 ? store1.sourceSystemId === store2.sourceSystemId : store1 === store2;
  }

  navigateToIntegrations() {
    this.store.dispatch(new Navigate([IntegrationsRoutePath.Integrations]));
  }

  protected readonly AuthRoles = AuthRoles;
}
