import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { RoutePaths } from '../../modules/app-routing.module';
import { environment } from '../../../../environments/environment';
import { AuthRoles } from '../../../shared/auth/models/auth.roles';
import { ToolbarComponent } from '../toolbar/toolbar.component';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { NgIf } from '@angular/common';
import { AuthRoleAllowDirective } from '../../../shared/auth/directives/auth-role-allow.directive';
import { MaterialModule } from '../../../shared/modules/material.module';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
  imports: [MaterialModule, ToolbarComponent, RouterLinkActive, RouterLink, NgIf, AuthRoleAllowDirective, RouterOutlet],
})
export class NavigationComponent {
  @ViewChild('sidenav') sidenav!: MatSidenav;

  RoutePaths = RoutePaths;
  AuthRoles = AuthRoles;

  get appVersion() {
    return environment.version;
  }
}
