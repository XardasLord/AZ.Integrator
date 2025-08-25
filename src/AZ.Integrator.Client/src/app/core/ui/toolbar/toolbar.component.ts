import { Component, EventEmitter, inject, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { KeycloakService } from 'keycloak-angular';
import { AuthState } from '../../../shared/states/auth.state';
import { environment } from '../../../../environments/environment';
import { AuthorizationProvider, Tenant, TenantGroup } from '../../../shared/auth/models/tenant.model';
import { ChangeTenant } from '../../../shared/states/tenant.action';
import { RoutePaths } from '../../modules/app-routing.module';
import { LoadNew } from '../../../features/orders/states/orders.action';
import { AuthRoles } from '../../../shared/auth/models/auth.roles';
import { AuthRoleAllowDirective } from '../../../shared/auth/directives/auth-role-allow.directive';
import { AsyncPipe, NgIf } from '@angular/common';
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

  tenantGroups: TenantGroup[] = [
    {
      groupName: 'ALLEGRO',
      tenants: [
        {
          tenantId: environment.allegroAzTeamTenantId,
          displayName: 'AZ TEAM',
          authorizationProvider: AuthorizationProvider.Allegro,
          isTestAccount: false,
        },
        {
          tenantId: environment.allegroMebleplTenantId,
          displayName: 'meblepl_24',
          authorizationProvider: AuthorizationProvider.Allegro,
          isTestAccount: false,
        },
        {
          tenantId: environment.allegroMyTestTenantId,
          displayName: 'MY TEST',
          authorizationProvider: AuthorizationProvider.Allegro,
          isTestAccount: true,
        },
      ],
    },
    {
      groupName: 'ERLI',
      tenants: [
        {
          tenantId: environment.erliAzTeamTenantId,
          displayName: 'AZ TEAM',
          authorizationProvider: AuthorizationProvider.Erli,
          isTestAccount: false,
        },
      ],
    },
    {
      groupName: 'SHOPIFY',
      tenants: [
        {
          tenantId: environment.shopifyUmeblovaneTenantId,
          displayName: 'Umeblovane',
          authorizationProvider: AuthorizationProvider.Shopify,
          isTestAccount: false,
        },
      ],
    },
  ];

  toggleMenu(): void {
    this.toggleSideNav.emit(true);
  }

  changeTenant(tenant: Tenant) {
    this.store.dispatch(new ChangeTenant(tenant));

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

  logout() {
    this.keycloak.logout();
  }

  protected readonly environment = environment;
  protected readonly AuthRoles = AuthRoles;
}
