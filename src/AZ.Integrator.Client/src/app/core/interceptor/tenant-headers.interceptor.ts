import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { TenantState } from '../../shared/states/tenant.state';

@Injectable()
export class TenantHeadersInterceptor implements HttpInterceptor {
  constructor(private store: Store) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const tenant = this.store.selectSnapshot(TenantState.getTenant);

    if (tenant) {
      req = req.clone({
        headers: req.headers
          .set('Az-Integrator-Tenant-Id', tenant.tenantId)
          .set('Az-Integrator-Shop-Provider', tenant.authorizationProvider.toString()),
      });
    }

    return next.handle(req);
  }
}
