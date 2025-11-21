import { Component, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { AuthState } from '../../../shared/states/auth.state';
import { AuthRoles } from '../../../shared/auth/models/auth.roles';
import { MarketplaceRoutePath, StocksRoutePath } from '../../modules/app-routing.module';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  imports: [MatButton, MatIcon, DatePipe],
})
export class HomeComponent {
  private store = inject(Store);

  today = new Date();

  startWorkRedirection() {
    const roles = this.store.selectSnapshot(AuthState.getUserRoles);

    if (roles.includes(AuthRoles.MasterAdmin)) {
      // TODO: Change to MasterAdmin specific route when available
    } else if (roles.includes(AuthRoles.Admin)) {
      this.store.dispatch(new Navigate([MarketplaceRoutePath.Orders]));
    } else if (roles.includes(AuthRoles.ScannerIn)) {
      this.store.dispatch(new Navigate([StocksRoutePath.BarcodeScanner]));
    } else if (roles.includes(AuthRoles.ScannerOut)) {
      this.store.dispatch(new Navigate([StocksRoutePath.BarcodeScanner]));
    }
  }
}
