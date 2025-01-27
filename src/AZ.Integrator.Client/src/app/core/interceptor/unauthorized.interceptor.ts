import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      tap(
        (event: HttpEvent<any>) => {
          if (event instanceof HttpResponse) {
            // Tutaj możesz przetwarzać odpowiedź HTTP
          }
        },
        error => {
          if (error.status === 401) {
            console.warn('Unauthorized access');
            // this.store.dispatch(new Relog());
          }
        }
      )
    );
  }
}
