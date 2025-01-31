import { Component, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { AuthState } from '../../../shared/states/auth.state';
import { AuthRoles } from '../../../shared/auth/models/auth.roles';
import { RoutePaths } from '../../modules/app-routing.module';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss'],
    imports: [MatButton, MatIcon]
})
export class HomeComponent {
  private store = inject(Store);


  startWorkRedirection() {
    const roles = this.store.selectSnapshot(AuthState.getUserRoles);

    if (roles.includes(AuthRoles.MasterAdmin)) {
      this.store.dispatch(new Navigate([RoutePaths.Orders]));
    } else if (roles.includes(AuthRoles.Admin)) {
      this.store.dispatch(new Navigate([RoutePaths.Orders]));
    } else if (roles.includes(AuthRoles.ScannerIn)) {
      console.warn('Redirection to Stocks IN');
    } else if (roles.includes(AuthRoles.ScannerOut)) {
      console.warn('Redirection to Stocks OUT');
    }
  }
}
