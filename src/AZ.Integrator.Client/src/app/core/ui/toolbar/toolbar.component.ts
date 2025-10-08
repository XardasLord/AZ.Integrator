import { Component, EventEmitter, inject, Output } from '@angular/core';
import { AsyncPipe, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { KeycloakService } from 'keycloak-angular';
import { AuthState } from '../../../shared/states/auth.state';
import { environment } from '../../../../environments/environment';
import {
  AuthorizationProvider,
  SourceSystem,
  SourceSystemGroup,
} from '../../../shared/auth/models/source-system.model';
import { ChangeSourceSystem } from '../../../shared/states/source-system.action';
import { RoutePaths } from '../../modules/app-routing.module';
import { LoadNew } from '../../../features/orders/states/orders.action';
import { AuthRoles } from '../../../shared/auth/models/auth.roles';
import { AuthRoleAllowDirective } from '../../../shared/auth/directives/auth-role-allow.directive';
import { MaterialModule } from '../../../shared/modules/material.module';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
  imports: [MaterialModule, AuthRoleAllowDirective, NgIf, AsyncPipe],
})
export class ToolbarComponent {
  private store = inject(Store);
  private route = inject(Router);
  private keycloak = inject(KeycloakService);

  @Output()
  toggleSideNav: EventEmitter<boolean> = new EventEmitter();
  user$ = this.store.select(AuthState.getProfile);

  tenantGroups: SourceSystemGroup[] = [
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

  onSelected(tenant: SourceSystem) {
    this.changeTenant(tenant);
  }

  changeTenant(tenant: SourceSystem) {
    this.store.dispatch(new ChangeSourceSystem(tenant));

    switch (this.route.url) {
      case `/${RoutePaths.Orders}`:
        this.store.dispatch(new LoadNew());
        return;
    }

    // TODO: This can be moved to a MASTER_ADMIN role functionality to get access token for the tenant
    // if (tenant.authorizationProvider === AuthorizationProvider.Allegro) {
    //   window.location.href = `${environment.allegroLoginEndpoint}${tenant.tenantId}`;
    // }
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

  toggleMenu(): void {
    this.toggleSideNav.emit(true);
  }

  logout() {
    this.keycloak.logout();
  }

  protected readonly environment = environment;
  protected readonly AuthRoles = AuthRoles;
}
