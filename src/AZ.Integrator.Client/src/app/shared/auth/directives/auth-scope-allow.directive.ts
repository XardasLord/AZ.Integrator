import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Directive({
    selector: '[appScopeAllow]',
    standalone: false
})
export class AuthScopeAllowDirective {
  private templateRef = inject<TemplateRef<any>>(TemplateRef);
  private viewContainer = inject(ViewContainerRef);
  private authService = inject(AuthService);

  private inputScopes: number[] | undefined;

  @Input()
  set appScopeAllow(allowedScopes: number[]) {
    if (this.inputScopes) {
      return;
    }

    this.inputScopes = allowedScopes;

    this.validateUserAllowed();
  }

  private validateUserAllowed(): void {
    if (!this.inputScopes || this.inputScopes.length === 0) {
      this.viewContainer.clear();
      return;
    }

    const isUserAllowed = this.authService.isUserAllowedByScopes(this.inputScopes);

    if (isUserAllowed) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
