import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { RoutePaths } from '../../modules/app-routing.module';
import { environment } from '../../../../environments/environment';
import { AuthRoles } from '../../../shared/auth/models/auth.roles';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class NavigationComponent {
  @ViewChild('sidenav') sidenav!: MatSidenav;

  RoutePaths = RoutePaths;
  AuthRoles = AuthRoles;

  get appVersion() {
    return environment.version;
  }
}
