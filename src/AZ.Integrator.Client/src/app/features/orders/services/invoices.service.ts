import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { RemoteServiceBase } from '../../../shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';
import { GraphQLHelper } from '../../../shared/graphql/graphql.helper';
import { IntegratorQueryInvoicesArgs, InvoiceViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { GenerateInvoiceCommand } from '../models/commands/generate-invoice.command';
import { GetInvoicesGQL } from '../graphql-queries/get-invoices.graphql.query';

@Injectable()
export class InvoicesService extends RemoteServiceBase {
  private invoicesGqlQuery = inject(GetInvoicesGQL);

  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);

    super(httpClient);
  }

  getInvoices(
    filters: IntegratorQueryInvoicesArgs
  ): Observable<GraphQLResponseWithoutPaginationVo<InvoiceViewModel[]>> {
    return this.invoicesGqlQuery
      .watch(filters, GraphQLHelper.defaultGraphQLWatchQueryOptions)
      .valueChanges.pipe(map(x => x.data));
  }

  generateInvoice(command: GenerateInvoiceCommand): Observable<string> {
    return this.httpClient.post<string>(`${this.apiUrl}/invoices`, command);
  }
}
