import { Injectable, inject } from '@angular/core';
import { UrlTree } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { AuthState } from '../../shared/states/auth.state';

@Injectable({
  providedIn: 'root',
})
export class LoginScreenGuard {
  private store = inject(Store);


  public canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const isAuthenticated = this.store.selectSnapshot(AuthState.isAuthenticated);

    return !isAuthenticated;
  }
}
