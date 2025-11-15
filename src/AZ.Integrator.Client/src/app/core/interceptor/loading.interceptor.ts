import { inject, Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { finalize, Observable, tap } from 'rxjs';
import { ProgressSpinnerService } from '../../shared/services/progress-spinner.service';
import { SKIP_LOADING } from './loading-context.token';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  private progressSpinnerService = inject(ProgressSpinnerService);

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (request.context.get(SKIP_LOADING)) {
      return next.handle(request);
    }

    return next.handle(request).pipe(
      tap(() => {
        this.progressSpinnerService.showProgressSpinner();
      }),
      finalize(() => {
        this.progressSpinnerService.hideProgressSpinner();
      })
    );
  }
}
