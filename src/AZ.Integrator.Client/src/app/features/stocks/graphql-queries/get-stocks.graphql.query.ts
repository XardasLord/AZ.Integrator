import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { IntegratorQuery, StockLogViewModel, StockViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';

@Injectable({
  providedIn: 'root',
})
export class GetStocksGQL extends Query<GraphQLResponseWithoutPaginationVo<StockViewModel[]>> {
  override document = gql`
    query stocks(
      $where: StockViewModelFilterInput
    ) {
      result: ${nameof<IntegratorQuery>('stocks')}(
        where: $where
      ) {
            ${nameof<StockViewModel>('packageCode')}
            ${nameof<StockViewModel>('quantity')}
            ${nameof<StockViewModel>('logs')} {
              ${nameof<StockLogViewModel>('changeQuantity')}
              ${nameof<StockLogViewModel>('createdAt')}
              ${nameof<StockLogViewModel>('createdBy')}
            }
        }
      }
    `;
}
