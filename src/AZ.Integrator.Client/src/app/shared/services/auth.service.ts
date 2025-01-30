import { Injectable, OnDestroy, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { Subscription } from 'rxjs';
import { AuthRoles } from '../auth/models/auth.roles';
import { UserAllowTerms } from '../auth/models/route-auth.vo';
import { getAuthRolesFromToken } from '../auth/helpers/keycloak-token-roles.helper';
import { KeycloakService } from 'keycloak-angular';

@Injectable()
export class AuthService implements OnDestroy {
  private store = inject(Store);
  private keycloak = inject(KeycloakService);

  private readonly roleSubscription: Subscription | undefined;
  private userRoles!: AuthRoles[];
  private userScopes!: number[];

  constructor() {
    this.userRoles = getAuthRolesFromToken(this.keycloak.getKeycloakInstance().tokenParsed!);
  }

  public ngOnDestroy(): void {
    if (this.roleSubscription) {
      this.roleSubscription.unsubscribe();
    }
  }

  isUserAllowed(roleRouteData: UserAllowTerms): boolean {
    let allowed = true;

    if (allowed && roleRouteData.allowScopes) {
      allowed = this.isUserAllowedByScopes(roleRouteData.allowScopes);
    }

    if (allowed && roleRouteData.allowRoles) {
      allowed = this.isUserAllowedByRoles(roleRouteData.allowRoles);
    }

    return allowed;
  }

  public isUserAllowedByScopes(scopes: number[]): boolean {
    if (!scopes) {
      return false;
    }

    return scopes.filter(scope => scope != null).some(scope => this.userScopes.includes(scope));
  }

  public isUserAllowedByRoles(roles: AuthRoles[]): boolean {
    if (this.userRoles.includes(AuthRoles.MasterAdmin)) {
      return true;
    }

    if (!roles) {
      return false;
    }

    return roles.filter(scope => scope != null).some(scope => this.userRoles.includes(scope));
  }
}
