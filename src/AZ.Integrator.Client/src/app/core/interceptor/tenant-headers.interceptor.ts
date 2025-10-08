import { inject, Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SourceSystemState } from '../../shared/states/source-system.state';

@Injectable()
export class TenantHeadersInterceptor implements HttpInterceptor {
  private store = inject(Store);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const tenant = this.store.selectSnapshot(SourceSystemState.getSourceSystem);

    if (tenant) {
      req = req.clone({
        headers: req.headers
          .set('Az-Integrator-Shop-Provider', tenant.authorizationProvider.toString())
          .set('Az-Integrator-Source-System-Id', tenant.sourceSystemId),
      });
    }

    return next.handle(req);
  }
}
