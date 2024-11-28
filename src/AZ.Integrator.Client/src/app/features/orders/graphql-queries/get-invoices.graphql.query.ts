import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { IntegratorQuery, InvoiceViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';

@Injectable({
  providedIn: 'root',
})
export class GetInvoicesGQL extends Query<GraphQLResponseWithoutPaginationVo<InvoiceViewModel[]>> {
  override document = gql`
    query invoices(
      $where: InvoiceViewModelFilterInput
    ) {
      result: ${nameof<IntegratorQuery>('invoices')}(
        where: $where
      ) {
            ${nameof<InvoiceViewModel>('invoiceId')}
            ${nameof<InvoiceViewModel>('invoiceNumber')}
            ${nameof<InvoiceViewModel>('externalOrderNumber')}
            ${nameof<InvoiceViewModel>('createdAt')}
        }
      }
    `;
}
