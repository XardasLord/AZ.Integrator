import { Injectable, NgZone } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { IntegratorQueryInvoicesArgs, InvoiceViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { IntegratorError } from '../../../core/interceptor/error-handler.interceptor';
import { GenerateInvoiceCommand } from '../models/commands/generate-invoice.command';
import { InvoicesStateModel } from './invoices.state.model';
import { GenerateInvoice, LoadInvoices } from './invoices.action';
import { InvoicesService } from '../services/invoices.service';

const INVOICES_STATE_TOKEN = new StateToken<InvoicesStateModel>('invoices');

@State<InvoicesStateModel>({
  name: INVOICES_STATE_TOKEN,
  defaults: {
    invoices: [],
  },
})
@Injectable()
export class InvoicesState {
  constructor(
    private invoicesService: InvoicesService,
    private zone: NgZone,
    private toastService: ToastrService
  ) {}

  @Selector([INVOICES_STATE_TOKEN])
  static getInvoices(state: InvoicesStateModel): InvoiceViewModel[] {
    return state.invoices;
  }

  @Action(LoadInvoices)
  loadInvoices(ctx: StateContext<InvoicesStateModel>, action: LoadInvoices) {
    let filters: IntegratorQueryInvoicesArgs = {};

    if (action.allegroOrderIds.length > 0) {
      filters = {
        where: {
          allegroOrderNumber: {
            in: action.allegroOrderIds,
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
      allegroOrderNumber: action.allegroOrderNumber,
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
}
