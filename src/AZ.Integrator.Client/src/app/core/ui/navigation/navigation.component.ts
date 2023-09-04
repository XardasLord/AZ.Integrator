import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { RoutePaths } from '../../modules/app-routing.module';
import { AuthScopes } from '../../../shared/auth/models/auth.scopes';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class NavigationComponent {
  @ViewChild('sidenav') sidenav!: MatSidenav;

  RoutePaths = RoutePaths;
  AuthScopes = AuthScopes;

  get appVersion() {
    return environment.version;
  }
}
