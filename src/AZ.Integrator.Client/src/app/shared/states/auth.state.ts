import { inject, Injectable } from '@angular/core';
import { Action, NgxsAfterBootstrap, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';

import Keycloak from 'keycloak-js';
import { KeycloakProfile } from '../../types/keycloak-js';

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
  },
})
@Injectable()
export class AuthState implements NgxsAfterBootstrap {
  private keycloak = inject(Keycloak);

  ngxsAfterBootstrap(ctx: StateContext<AuthStateModel>): void {
    if (this.keycloak.authenticated) {
      ctx.dispatch(new LoginCompleted());
    } else {
      ctx.dispatch(new Logout());
    }
  }

  @Selector([AUTH_STATE_TOKEN])
  static getProfile(state: AuthStateModel): KeycloakProfile | null {
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
    // if (!state || !state) {
    //   return [];
    // }

    return Object.values(state.authRoles);
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
    // TODO: Extend profile with the custom app_scopes
    this.keycloak.loadUserProfile().then(profile => {
      ctx.patchState({
        isLoggedIn: this.keycloak.authenticated,
        profile: profile,
        authRoles: getAuthRolesFromToken(this.keycloak.idTokenParsed!),
      });
    });

    ctx.dispatch(new Navigate([RoutePaths.Home]));
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
