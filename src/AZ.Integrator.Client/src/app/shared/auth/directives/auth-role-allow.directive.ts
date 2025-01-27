import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AuthRoles } from '../models/auth.roles';
import { Store } from '@ngxs/store';
import { KeycloakService } from 'keycloak-angular';

@Directive({
  selector: '[appRoleAllow]',
})
export class AuthRoleAllowDirective {
  private inputRoles: AuthRoles[] | undefined;

  constructor(
    private store: Store,
    private keycloak: KeycloakService,
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) {}

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
