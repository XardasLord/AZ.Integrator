import { Component, EventEmitter, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { LoginViaErli, Logout } from '../../../shared/states/auth.action';
import { AuthState } from '../../../shared/states/auth.state';
import { environment } from '../../../../environments/environment';
import { AuthorizationProvider, Tenant } from '../../../shared/auth/models/tenant.model';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent {
  @Output()
  toggleSideNav: EventEmitter<boolean> = new EventEmitter();
  user$ = this.store.select(AuthState.getUser);

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

  constructor(private store: Store) {}

  toggleMenu(): void {
    this.toggleSideNav.emit(true);
  }

  login(tenant: Tenant) {
    this.store.dispatch(new Logout());

    if (tenant.authorizationProvider === AuthorizationProvider.Allegro) {
      window.location.href = `${environment.allegroLoginEndpoint}${tenant.tenantId}`;
    } else if (tenant.authorizationProvider === AuthorizationProvider.Erli) {
      this.store.dispatch(new LoginViaErli(tenant.tenantId));
    }
  }

  protected readonly environment = environment;
}
