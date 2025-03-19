import { Injectable, inject } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { finalize, Observable, tap } from 'rxjs';
import { ProgressSpinnerService } from '../../shared/services/progress-spinner.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  private progressSpinnerService = inject(ProgressSpinnerService);


  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
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
