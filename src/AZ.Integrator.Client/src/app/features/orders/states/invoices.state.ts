import { Injectable, NgZone, inject } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, of, switchMap, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { IntegratorQueryInvoicesArgs, InvoiceViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { IntegratorError } from '../../../core/interceptor/error-handler.interceptor';
import { GenerateInvoiceCommand } from '../models/commands/generate-invoice.command';
import { InvoicesStateModel } from './invoices.state.model';
import { DownloadInvoice, GenerateInvoice, LoadInvoices } from './invoices.action';
import { InvoicesService } from '../services/invoices.service';
import { OrdersStateModel } from './orders-state.model';
import { DownloadService } from '../../../shared/services/download.service';

const INVOICES_STATE_TOKEN = new StateToken<InvoicesStateModel>('invoices');

@State<InvoicesStateModel>({
  name: INVOICES_STATE_TOKEN,
  defaults: {
    invoices: [],
  },
})
@Injectable()
export class InvoicesState {
  private invoicesService = inject(InvoicesService);
  private downloadService = inject(DownloadService);
  private zone = inject(NgZone);
  private toastService = inject(ToastrService);


  @Selector([INVOICES_STATE_TOKEN])
  static getInvoices(state: InvoicesStateModel): InvoiceViewModel[] {
    return state.invoices;
  }

  @Action(LoadInvoices)
  loadInvoices(ctx: StateContext<InvoicesStateModel>, action: LoadInvoices) {
    let filters: IntegratorQueryInvoicesArgs = {};

    if (action.orderIds.length > 0) {
      filters = {
        where: {
          externalOrderNumber: {
            in: action.orderIds,
          },
        },
      };
    }
    return this.invoicesService.getInvoices(filters).pipe(
      tap(response => {
        ctx.patchState({
          invoices: response.result,
        });
      })
    );
  }

  @Action(GenerateInvoice)
  generateInvoice(ctx: StateContext<InvoicesStateModel>, action: GenerateInvoice) {
    const command: GenerateInvoiceCommand = {
      orderNumber: action.orderNumber,
    };

    return this.invoicesService.generateInvoice(command).pipe(
      tap(() => {
        this.zone.run(() => this.toastService.success(`Faktura VAT została wygenerowana`, 'Faktura VAT'));

        ctx.dispatch(new LoadInvoices());
      }),
      catchError(error => {
        const applicationError: IntegratorError = error.error;

        this.zone.run(() =>
          this.toastService.error(`Błąd podczas generowania faktury VAT - ${applicationError.Message}`, 'Faktura VAT')
        );
        return throwError(error);
      })
    );
  }

  @Action(DownloadInvoice)
  downloadInvoice(ctx: StateContext<OrdersStateModel>, action: DownloadInvoice) {
    return this.downloadService.downloadFileFromApi(`/invoices/${action.invoiceId}`).pipe(
      switchMap(resBlob => {
        this.downloadService.getFile(resBlob, `${action.invoiceNumber}.pdf`);
        this.toastService.success('Faktura została pobrana', 'Faktura VAT');

        return of(null);
      }),
      catchError(() => {
        this.toastService.error(`Błąd podczas pobierania faktury VAT`, 'Faktura VAT');

        return of(null);
      })
    );
  }
}
