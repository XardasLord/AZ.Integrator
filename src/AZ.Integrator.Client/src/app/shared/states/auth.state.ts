import { Inject, Injectable } from '@angular/core';
import { Action, NgxsOnInit, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { map, tap } from 'rxjs';
import { UserAuthModel } from '../auth/models/user-auth.model';
import { AuthScopes } from '../auth/models/auth.scopes';
import { Login, LoginCompleted, Logout } from './auth.action';
import { RoutePaths } from '../../core/modules/app-routing.module';
import { AuthService } from '../services/auth.service';
import { AuthStateModel } from './auth.state.model';
import { User } from 'oidc-client';
import { UserAuthHelper } from '../auth/helpers/user-auth.helper';
import { DOCUMENT } from '@angular/common';

export const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');

@State<AuthStateModel>({
  name: AUTH_STATE_TOKEN,
  defaults: {
    user: null,
  },
})
@Injectable()
export class AuthState implements NgxsOnInit {
  constructor(
    private authService: AuthService,
    @Inject(DOCUMENT) private document: Document
  ) {}

  ngxsOnInit(ctx: StateContext<AuthStateModel>): void {
    const currentUrl = this.document.location.href;
    const regExp = new RegExp(`(access_token=)(.+)`).exec(currentUrl);
    if (regExp && regExp[2]) {
      console.warn(regExp[2]);
      // TODO: Logowanie zakończone. Zapisz token
      // return ctx.dispatch(new CallbackSelected());
    } else {
      // TODO: Zaloguj
      console.warn('BRAK');
    }

    const user = AuthState.getUser(ctx.getState());
    if (!user) {
      ctx.dispatch(new Navigate([RoutePaths.Login]));
    }

    ctx.patchState({
      user: user,
    });
  }

  @Selector([AUTH_STATE_TOKEN])
  static getUser(state: AuthStateModel): UserAuthModel | null {
    if (state?.user === null && localStorage.getItem('user') !== null) {
      return JSON.parse(localStorage.getItem('user')!);
    }

    return state?.user;
  }

  @Selector([AUTH_STATE_TOKEN])
  static isAuthenticated(state: AuthStateModel): boolean {
    return !!state.user;
  }

  @Selector([AUTH_STATE_TOKEN])
  static getUserScopes(state: AuthStateModel): AuthScopes[] {
    if (!state || !state.user || !state.user.auth_scopes) {
      return [];
    }

    return Object.values(state.user.auth_scopes).map(x => +x as number);
  }

  @Action(Login)
  login(ctx: StateContext<AuthStateModel>, action: Login) {
    return this.authService.login(action.login, action.password).pipe(
      map((user: User) => {
        const authUser = UserAuthHelper.parseUserAuthData(user);

        if (authUser) {
          localStorage.setItem('access_token', user.access_token);
          localStorage.setItem('user', JSON.stringify(authUser));

          // const expiresAt = moment().add(authResult.expiresIn,'second');
          // localStorage.setItem("expires_at", JSON.stringify(expiresAt.valueOf()) );

          ctx.dispatch(new LoginCompleted(authUser));
        }
      })
    );
  }

  @Action(LoginCompleted)
  loginCompleted(ctx: StateContext<AuthStateModel>, action: LoginCompleted): void {
    ctx.patchState({
      user: action.response,
    });

    ctx.dispatch(new Navigate([RoutePaths.Test]));
  }

  @Action(Logout)
  logout(ctx: StateContext<AuthStateModel>, _: Logout) {
    return this.authService.logout().pipe(
      tap(() => {
        localStorage.removeItem('access_token');
        localStorage.removeItem('user');
        // localStorage.removeItem("expires_at");

        ctx.patchState({
          user: null,
        });

        ctx.dispatch(new Navigate([RoutePaths.Login]));
      })
    );
  }
}
