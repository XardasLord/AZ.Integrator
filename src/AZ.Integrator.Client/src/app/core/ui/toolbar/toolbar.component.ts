import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { KeycloakService } from 'keycloak-angular';
import { AuthState } from '../../../shared/states/auth.state';
import { environment } from '../../../../environments/environment';
import { AuthorizationProvider, Tenant } from '../../../shared/auth/models/tenant.model';
import { ChangeTenant } from '../../../shared/states/tenant.action';
import { RoutePaths } from '../../modules/app-routing.module';
import { LoadNew } from '../../../features/orders/states/orders.action';
import { LoadProductTags } from '../../../features/package-templates/states/parcel-templates.action';
import { AuthRoles } from '../../../shared/auth/models/auth.roles';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent {
  @Output()
  toggleSideNav: EventEmitter<boolean> = new EventEmitter();
  user$ = this.store.select(AuthState.getProfile);

  tenants: Tenant[] = [
    {
      tenantId: environment.allegroAzTeamTenantId,
      displayName: 'ALLEGRO - AZ TEAM',
      authorizationProvider: AuthorizationProvider.Allegro,
      isTestAccount: false,
    },
    {
      tenantId: environment.allegroMebleplTenantId,
      displayName: 'ALLEGRO - meblepl_24',
      authorizationProvider: AuthorizationProvider.Allegro,
      isTestAccount: false,
    },
    {
      tenantId: environment.allegroMyTestTenantId,
      displayName: 'ALLEGRO - MY TEST',
      authorizationProvider: AuthorizationProvider.Allegro,
      isTestAccount: true,
    },
    {
      tenantId: environment.erliAzTeamTenantId,
      displayName: 'ERLI - AZ TEAM',
      authorizationProvider: AuthorizationProvider.Erli,
      isTestAccount: false,
    },
  ];

  constructor(
    private store: Store,
    private route: Router,
    private keycloak: KeycloakService
  ) {}

  toggleMenu(): void {
    this.toggleSideNav.emit(true);
  }

  changeTenant(tenant: Tenant) {
    this.store.dispatch(new ChangeTenant(tenant));

    switch (this.route.url) {
      case `/${RoutePaths.Orders}`:
        this.store.dispatch(new LoadNew());
        return;
      case `/${RoutePaths.ParcelTemplates}`:
        this.store.dispatch(new LoadProductTags());
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
