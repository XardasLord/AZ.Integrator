import { ErrorHandler, inject, Injectable, NgZone } from '@angular/core';
import { ErrorService } from '../../shared/errors/error.service';
import { MessageStatusEnum } from '../../shared/errors/message-status.enum';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  private errorService = inject(ErrorService);
  private zone = inject(NgZone);

  handleError(error: HttpErrorResponse): void {
    const applicationError: IntegratorError = error?.error;

    this.zone.run(() => this.errorService.display(applicationError?.Message, MessageStatusEnum.Error));
    console.error('Error from global error handler', error);
  }
}

export interface IntegratorError {
  Code: string;
  Message: string;
  AdditionalMessage: string;
  HttpStatusCode: number;
}
