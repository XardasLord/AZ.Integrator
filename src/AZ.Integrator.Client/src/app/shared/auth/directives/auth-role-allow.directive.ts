import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AuthRoles } from '../models/auth.roles';
import { Store } from '@ngxs/store';
import { KeycloakService } from 'keycloak-angular';

@Directive({ selector: '[appRoleAllow]' })
export class AuthRoleAllowDirective {
  private store = inject(Store);
  private keycloak = inject(KeycloakService);
  private templateRef = inject<TemplateRef<any>>(TemplateRef);
  private viewContainer = inject(ViewContainerRef);
  private authService = inject(AuthService);

  private inputRoles: AuthRoles[] | undefined;

  @Input()
  set appRoleAllow(allowedRoles: AuthRoles[]) {
    if (this.inputRoles) {
      return;
    }

    this.inputRoles = allowedRoles;

    this.validateUserAllowed();
  }

  private validateUserAllowed(): void {
    if (!this.inputRoles || this.inputRoles.length === 0) {
      this.viewContainer.clear();
      return;
    }

    const isUserAllowed = this.authService.isUserAllowedByRoles(this.inputRoles);

    if (isUserAllowed) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
