import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import {
  IntegratorQuery,
  StockGroupViewModel,
  StockViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';

@Injectable({
  providedIn: 'root',
})
export class GetStockGroupsGQL extends Query<GraphQLResponseWithoutPaginationVo<StockGroupViewModel[]>> {
  override document = gql`
    query stockGroups(
      $where: StockGroupViewModelFilterInput, $order: [StockGroupViewModelSortInput!]!
    ) {
      result: ${nameof<IntegratorQuery>('stockGroups')}(
        where: $where, order: $order
      ) {
            ${nameof<StockGroupViewModel>('id')}
            ${nameof<StockGroupViewModel>('name')}
            ${nameof<StockGroupViewModel>('description')}
            ${nameof<StockGroupViewModel>('stocks')} {
              ${nameof<StockViewModel>('packageCode')}
              ${nameof<StockViewModel>('quantity')}
            }
        }
      }
    `;
}
