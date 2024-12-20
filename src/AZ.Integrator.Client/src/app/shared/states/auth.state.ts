import { Inject, Injectable } from '@angular/core';
import { Action, NgxsOnInit, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { tap } from 'rxjs';
import { UserAuthModel } from '../auth/models/user-auth.model';
import { AuthScopes } from '../auth/models/auth.scopes';
import { Login, LoginCompleted, LoginViaErli, Logout, Relog } from './auth.action';
import { AuthService } from '../services/auth.service';
import { AuthStateModel } from './auth.state.model';
import { UserAuthHelper } from '../auth/helpers/user-auth.helper';
import { DOCUMENT } from '@angular/common';
import { RoutePaths } from '../../core/modules/app-routing.module';
import { ApplyFilter } from '../../features/orders/states/orders.action';

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

      const authUser = UserAuthHelper.parseAccessToken(accessToken);

      ctx.dispatch(new LoginCompleted(authUser!));
    } else {
      console.warn('BRAK TOKENU - wymagane zalogowanie');
      // ctx.dispatch(new Login());
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
    // TODO: Unused?
  }

  @Action(LoginCompleted)
  loginCompleted(ctx: StateContext<AuthStateModel>, action: LoginCompleted) {
    ctx.patchState({
      user: action.user,
    });

    localStorage.setItem('access_token', action.user.access_token!);
    localStorage.setItem('user', JSON.stringify(action.user));

    ctx.dispatch(new Navigate([RoutePaths.Orders]));
    ctx.dispatch(new ApplyFilter('')); // This is temporary solution to reload list of orders
  }

  @Action(LoginViaErli)
  loginViaErli(ctx: StateContext<AuthStateModel>, action: LoginViaErli) {
    return this.authService.loginViaErli(action.tenantId).pipe(
      tap((response: { access_token: string }) => {
        const authUser = UserAuthHelper.parseAccessToken(response.access_token);

        if (authUser) {
          ctx.dispatch(new LoginCompleted(authUser));
        }
      })
    );
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

        // ctx.dispatch(new Login());
        // ctx.dispatch(new Navigate([RoutePaths.Login]));
      })
    );
  }

  @Action(Relog)
  relog(ctx: StateContext<AuthStateModel>) {
    return ctx.dispatch(new Logout());
  }
}
