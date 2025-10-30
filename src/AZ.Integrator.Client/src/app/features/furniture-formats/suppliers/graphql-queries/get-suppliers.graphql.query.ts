import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { GraphQLResponse } from 'src/app/shared/graphql/graphql.response';
import {
  IntegratorQuery,
  PageInfo,
  SupplierMailboxViewModel,
  SuppliersConnection,
  SupplierViewModel,
} from '../../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../../shared/helpers/name-of.helper';

@Injectable({
  providedIn: 'root',
})
export class GetSuppliersGQL extends Query<GraphQLResponse<SupplierViewModel[]>> {
  override document = gql`
    query suppliers(
      $after: String
      $before: String
      $first: Int
      $last: Int
      $where: SupplierViewModelFilterInput
      $order: [SupplierViewModelSortInput!]
    ) {
      result: ${nameof<IntegratorQuery>('suppliers')}(
        after: $after
        before: $before
        first: $first
        last: $last
        where: $where
        order: $order
      ) {
        ${nameof<SuppliersConnection>('nodes')} {
          ${nameof<SupplierViewModel>('id')}
          ${nameof<SupplierViewModel>('name')}
          ${nameof<SupplierViewModel>('telephoneNumber')}
          ${nameof<SupplierViewModel>('createdAt')}
          ${nameof<SupplierViewModel>('createdBy')}
          ${nameof<SupplierViewModel>('modifiedAt')}
          ${nameof<SupplierViewModel>('modifiedBy')}
          ${nameof<SupplierViewModel>('tenantId')}

          ${nameof<SupplierViewModel>('mailboxes')} {
            ${nameof<SupplierMailboxViewModel>('id')}
            ${nameof<SupplierMailboxViewModel>('email')}
            ${nameof<SupplierMailboxViewModel>('supplierId')}
          }
        }
        ${nameof<SuppliersConnection>('totalCount')}
        ${nameof<SuppliersConnection>('pageInfo')} {
          ${nameof<PageInfo>('startCursor')}
          ${nameof<PageInfo>('endCursor')}
          ${nameof<PageInfo>('hasNextPage')}
          ${nameof<PageInfo>('hasPreviousPage')}
        }
      }
    }
  `;
}
