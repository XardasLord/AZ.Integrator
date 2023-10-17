import { Inject, Injectable } from '@angular/core';
import { Action, NgxsOnInit, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { map, tap } from 'rxjs';
import { UserAuthModel } from '../auth/models/user-auth.model';
import { AuthScopes } from '../auth/models/auth.scopes';
import { Login, LoginCompleted, Logout, Relog } from './auth.action';
import { RoutePaths } from '../../core/modules/app-routing.module';
import { AuthService } from '../services/auth.service';
import { AuthStateModel } from './auth.state.model';
import { User } from 'oidc-client';
import { UserAuthHelper } from '../auth/helpers/user-auth.helper';
import { DOCUMENT } from '@angular/common';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../../environments/environment';

export const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');

@State<AuthStateModel>({
  name: AUTH_STATE_TOKEN,
  defaults: {
    user: JSON.parse(localStorage.getItem('user')!),
  },
})
@Injectable()
export class AuthState implements NgxsOnInit {
  constructor(
    private authService: AuthService,
    @Inject(DOCUMENT) private document: Document
  ) {}

  ngxsOnInit(ctx: StateContext<AuthStateModel>): void {
    const state = ctx.getState();
    if (state.user) {
      ctx.dispatch(new LoginCompleted(state.user));
      return;
    }

    const currentUrl = this.document.location.href;
    const regExp = new RegExp(`(access_token=)(.+)`).exec(currentUrl);

    if (regExp && regExp[2]) {
      const accessToken = regExp[2];
      console.warn(accessToken);

      const authUser = UserAuthHelper.parseAccessToken(accessToken);

      ctx.dispatch(new LoginCompleted(authUser!));
    } else {
      console.warn('BRAK TOKENU - wymagane zalogowanie');
      ctx.dispatch(new Login());
    }

    // const user = AuthState.getUser(ctx.getState());
    // if (!user) {
    //   ctx.dispatch(new Navigate([RoutePaths.Login]));
    // }
    //
    // ctx.patchState({
    //   user: user,
    // });
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
  login(ctx: StateContext<AuthStateModel>, _: Login) {
    const authUrl = `${environment.allegroLoginEndpoint}`;
    window.location.href = authUrl;
    // return this.authService.login(action.login, action.password).pipe(
    //   map((user: User) => {
    //     const authUser = UserAuthHelper.parseAccessToken(user);
    //
    //     if (authUser) {
    //       localStorage.setItem('access_token', user.access_token);
    //       localStorage.setItem('user', JSON.stringify(authUser));
    //
    //       // const expiresAt = moment().add(authResult.expiresIn,'second');
    //       // localStorage.setItem("expires_at", JSON.stringify(expiresAt.valueOf()) );
    //
    //       ctx.dispatch(new LoginCompleted(authUser));
    //     }
    //   })
    // );
  }

  @Action(LoginCompleted)
  loginCompleted(ctx: StateContext<AuthStateModel>, action: LoginCompleted): void {
    ctx.patchState({
      user: action.user,
    });

    localStorage.setItem('access_token', action.user.access_token!);
    localStorage.setItem('user', JSON.stringify(action.user));

    ctx.dispatch(new Navigate([RoutePaths.AllegroOrders]));
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

        ctx.dispatch(new Login());
        // ctx.dispatch(new Navigate([RoutePaths.Login]));
      })
    );
  }

  @Action(Relog)
  relog(ctx: StateContext<AuthStateModel>) {
    return ctx.dispatch(new Logout());
  }
}
