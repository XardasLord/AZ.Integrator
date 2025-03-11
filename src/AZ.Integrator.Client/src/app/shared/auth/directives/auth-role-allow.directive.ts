import { Directive, inject, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AuthRoles } from '../models/auth.roles';

@Directive({ selector: '[appRoleAllow]' })
export class AuthRoleAllowDirective {
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
    const isUserAllowed = this.authService.isUserAllowedByRoles(this.inputRoles);

    if (isUserAllowed) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
