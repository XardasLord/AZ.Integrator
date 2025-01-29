import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Directive({
    selector: '[appScopeAllow]',
    standalone: false
})
export class AuthScopeAllowDirective {
  private inputScopes: number[] | undefined;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) {}

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
