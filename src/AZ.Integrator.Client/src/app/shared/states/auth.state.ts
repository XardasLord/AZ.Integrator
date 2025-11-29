import { inject, Injectable } from '@angular/core';
import { Action, NgxsAfterBootstrap, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';

import Keycloak, { KeycloakProfile } from 'keycloak-js';
import { AppKeycloakProfile, AppKeycloakUserAttributes } from '../../types/keycloak-js';

import { AuthScopes } from '../auth/models/auth.scopes';
import { Login, LoginCompleted, Logout, NotAuthorized, Relog } from './auth.action';
import { AuthStateModel } from './auth.state.model';
import { RoutePaths } from '../../core/modules/app-routing.module';
import { AuthRoles } from '../auth/models/auth.roles';
import { getAuthRolesFromToken } from '../auth/helpers/keycloak-token-roles.helper';

export const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');

@State<AuthStateModel>({
  name: AUTH_STATE_TOKEN,
  defaults: {
    profile: null,
    isLoggedIn: false,
    authScopes: [],
    authRoles: [],
    redirectUrl: undefined,
  },
})
@Injectable()
export class AuthState implements NgxsAfterBootstrap {
  private keycloak = inject(Keycloak);

  ngxsAfterBootstrap(ctx: StateContext<AuthStateModel>): void {
    const currentUrl = window.location.pathname + window.location.search;
    const hasConnectionParam = currentUrl.includes('_connected');
    const isAuthenticated = this.keycloak.authenticated;

    if (isAuthenticated) {
      // User jest zalogowany - załaduj profil bez redirectu
      this.keycloak.loadUserProfile().then(keycloakProfile => {
        ctx.patchState({
          isLoggedIn: true,
          profile: mapProfile(keycloakProfile),
          authRoles: getAuthRolesFromToken(this.keycloak.idTokenParsed!),
        });
      });
    } else {
      // User NIE jest zalogowany
      // Zapisz URL z parametrem _connected jeśli istnieje (do użycia po zalogowaniu)
      if (hasConnectionParam && currentUrl && currentUrl !== '/' && !currentUrl.includes('/login')) {
        ctx.patchState({ redirectUrl: currentUrl });
      }
      ctx.dispatch(new Logout());
    }
  }

  @Selector([AUTH_STATE_TOKEN])
  static getProfile(state: AuthStateModel): AppKeycloakProfile | null {
    if (state?.profile === null && localStorage.getItem('profile') !== null) {
      return JSON.parse(localStorage.getItem('profile')!);
    }

    return state?.profile;
  }

  @Selector([AUTH_STATE_TOKEN])
  static isAuthenticated(state: AuthStateModel): boolean {
    return state.isLoggedIn;
  }

  @Selector([AUTH_STATE_TOKEN])
  static getUserScopes(state: AuthStateModel): AuthScopes[] {
    if (!state || !state.authScopes) {
      return [];
    }

    return Object.values(state.authScopes).map(x => +x as number);
  }

  @Selector([AUTH_STATE_TOKEN])
  static getUserRoles(state: AuthStateModel): AuthRoles[] {
    return Object.values(state.authRoles);
  }

  @Selector([AUTH_STATE_TOKEN])
  static getTenantId(state: AuthStateModel): string {
    return state.profile!.attributes!.tenant_id![0]!;
  }

  @Action(Login)
  login(ctx: StateContext<AuthStateModel>) {
    ctx.patchState({
      profile: undefined,
      isLoggedIn: false,
    });

    this.keycloak?.login({
      redirectUri: `${window.location.origin}/login-completed`,
    });
  }

  @Action(LoginCompleted)
  loginCompleted(ctx: StateContext<AuthStateModel>) {
    const state = ctx.getState();
    const redirectUrl = state.redirectUrl;

    // TODO: Extend profile with the custom app_scopes
    this.keycloak.loadUserProfile().then(keycloakProfile => {
      ctx.patchState({
        isLoggedIn: this.keycloak.authenticated,
        profile: mapProfile(keycloakProfile),
        authRoles: getAuthRolesFromToken(this.keycloak.idTokenParsed!),
      });

      // Przekieruj TYLKO jeśli mamy zapisany redirectUrl (czyli user nie był zalogowany i dopiero się zalogował)
      if (redirectUrl) {
        // Wyczyść zapisany URL
        ctx.patchState({ redirectUrl: undefined });
        // Użyj pełnego URL z query params - pełny reload strony aby Angular prawidłowo obsłużył routing
        window.location.href = redirectUrl;
      } else {
        // Jeśli nie ma redirectUrl, to user był już zalogowany - nie rób nic (zostaje na obecnej stronie)
        // LUB jeśli to pierwsze zalogowanie, przekieruj do HOME
        const currentPath = window.location.pathname;
        if (currentPath === '/' || currentPath === '/login-completed') {
          ctx.dispatch(new Navigate([RoutePaths.Home]));
        }
        // W przeciwnym razie zostaw usera gdzie jest
      }
    });
  }

  @Action(Logout)
  logout(ctx: StateContext<AuthStateModel>) {
    ctx.patchState({
      profile: null,
      isLoggedIn: false,
      authRoles: [],
    });

    this.keycloak.logout().then(() => {
      ctx.dispatch(new Login());
    });
  }

  @Action(Relog)
  relog(ctx: StateContext<AuthStateModel>) {
    return ctx.dispatch(new Logout());
  }

  @Action(NotAuthorized)
  notAuthorized(ctx: StateContext<AuthStateModel>) {
    ctx.dispatch(new Navigate([RoutePaths.NotAuthorized]));
  }
}

function mapProfile(profile: KeycloakProfile): AppKeycloakProfile {
  return {
    id: profile.id,
    username: profile.username,
    email: profile.email,
    firstName: profile.firstName,
    lastName: profile.lastName,
    enabled: profile.enabled,
    emailVerified: profile.emailVerified,
    totp: profile.totp,
    createdTimestamp: profile.createdTimestamp,
    attributes: profile.attributes as AppKeycloakUserAttributes | undefined,
  };
}
