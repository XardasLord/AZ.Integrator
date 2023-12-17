import { Component, EventEmitter, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { Logout } from '../../../shared/states/auth.action';
import { AuthState } from '../../../shared/states/auth.state';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent {
  @Output()
  toggleSideNav: EventEmitter<boolean> = new EventEmitter();
  user$ = this.store.select(AuthState.getUser);

  azTeamTenantUrl = environment.allegroLoginEndpointForAzTeamTenant;
  meblePlTenantUrl = environment.allegroLoginEndpointForMebleplTenant;
  myTestTenantUrl = environment.allegroLoginEndpointForMyTestTenant;

  constructor(private store: Store) {}

  toggleMenu(): void {
    this.toggleSideNav.emit(true);
  }

  login(tenantLoginUrl: any) {
    this.store.dispatch(new Logout());

    console.log(tenantLoginUrl);
    const authUrl = `${tenantLoginUrl}`;
    window.location.href = authUrl;
  }
}
