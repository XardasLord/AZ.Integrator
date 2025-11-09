import { Component, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ApplyFilter, LoadNew } from '../../states/orders.action';
import { OrdersState } from '../../states/orders-state.service';
import { DebounceDirective } from '../../../../shared/directives/debounce.directive';
import { AsyncPipe, NgIf } from '@angular/common';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { environment } from '../../../../../environments/environment';
import {
  AuthorizationProvider,
  SourceSystem,
  SourceSystemGroup,
} from '../../../../shared/auth/models/source-system.model';
import { ChangeSourceSystem } from '../../../../shared/states/source-system.action';
import { SourceSystemState } from '../../../../shared/states/source-system.state';
import { AuthRoles } from '../../../../shared/auth/models/auth.roles';
import { AuthRoleAllowDirective } from '../../../../shared/auth/directives/auth-role-allow.directive';

@Component({
  selector: 'app-orders-filters',
  templateUrl: './orders-filters.component.html',
  styleUrls: ['./orders-filters.component.scss'],
  imports: [MaterialModule, DebounceDirective, AsyncPipe, NgIf, AuthRoleAllowDirective],
})
export class OrdersFiltersComponent {
  private store = inject(Store);

  searchText$: Observable<string> = this.store.select(OrdersState.getSearchText);
  selectedStore$ = this.store.select(SourceSystemState.getSourceSystem);

  sourceSystemGroups: SourceSystemGroup[] = [
    {
      groupName: 'ALLEGRO',
      sourceSystems: [
        {
          sourceSystemId: environment.allegroAzTeamTenantId,
          displayName: 'AZ TEAM',
          subtitle: 'ALLEGRO',
          authorizationProvider: AuthorizationProvider.Allegro,
        },
        {
          sourceSystemId: environment.allegroMebleplTenantId,
          displayName: 'meblepl_24',
          subtitle: 'ALLEGRO',
          authorizationProvider: AuthorizationProvider.Allegro,
        },
      ],
    },
    {
      groupName: 'ERLI',
      sourceSystems: [
        {
          sourceSystemId: environment.erliAzTeamTenantId,
          displayName: 'AZ TEAM',
          subtitle: 'ERLI',
          authorizationProvider: AuthorizationProvider.Erli,
        },
      ],
    },
    {
      groupName: 'SHOPIFY',
      sourceSystems: [
        {
          sourceSystemId: environment.shopifyUmeblovaneTenantId,
          displayName: 'Umeblovane',
          subtitle: 'umeblovane.pl',
          authorizationProvider: AuthorizationProvider.Shopify,
        },
      ],
    },
  ];

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilter(searchText));
  }

  onStoreSelected(sourceSystem: SourceSystem) {
    this.store.dispatch(new ChangeSourceSystem(sourceSystem));
    this.store.dispatch(new LoadNew());
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

  protected readonly AuthRoles = AuthRoles;
}
