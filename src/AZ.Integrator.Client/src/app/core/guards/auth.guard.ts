import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { RoutePaths } from '../modules/app-routing.module';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  constructor(
    private store: Store,
    private router: Router
  ) {}

  canActivate() {
    if (localStorage.getItem('access_token')) {
      // logged in so return true
      return true;
    }

    // not logged in so redirect to login page with the return url
    this.store.dispatch(new Navigate([RoutePaths.Login], { returnUrl: this.router.url }));
    return false;
  }
}
