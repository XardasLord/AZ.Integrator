import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { IntegratorQuery, StockLogViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';

@Injectable({
  providedIn: 'root',
})
export class GetBarcodeScannerLogsGQL extends Query<GraphQLResponseWithoutPaginationVo<StockLogViewModel[]>> {
  override document = gql`
    query stocks(
      $where: StockLogViewModelFilterInput,
      $order: [StockLogViewModelSortInput!]!
    ) {
      result: ${nameof<IntegratorQuery>('barcodeScannerLogs')}(
        where: $where,
        order: $order
      ) {
            ${nameof<StockLogViewModel>('id')}
            ${nameof<StockLogViewModel>('packageCode')}
            ${nameof<StockLogViewModel>('changeQuantity')}
            ${nameof<StockLogViewModel>('status')}
            ${nameof<StockLogViewModel>('createdBy')}
            ${nameof<StockLogViewModel>('createdAt')}
        }
      }
    `;
}
