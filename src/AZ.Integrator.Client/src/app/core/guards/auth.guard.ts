import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Store } from '@ngxs/store';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { AuthService } from '../../shared/services/auth.service';
import { AuthState } from '../../shared/states/auth.state';
import { RouteAuthVo } from '../../shared/auth/models/route-auth.vo';
import { NotAuthorized } from '../../shared/states/auth.action';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  constructor(
    private store: Store,
    private authService: AuthService
  ) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.parseRoute(route);
  }

  private parseRoute(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.store.select(AuthState.getUserRoles).pipe(
      map(roles => {
        if (!roles) {
          return false;
        }

        const authData = route.data as RouteAuthVo;

        if (!authData) {
          return false;
        }

        return this.authService.isUserAllowed(authData);
      }),
      tap(canActivate => {
        if (!canActivate) {
          this.store.dispatch(new NotAuthorized());
        }
      }),
      catchError(error => {
        console.error({ error });
        return of(false);
      })
    );
  }
}
