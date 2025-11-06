import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { GraphQLResponse } from 'src/app/shared/graphql/graphql.response';
import {
  IntegratorQuery,
  OrderFurnitureLineViewModel,
  OrderFurniturePartLineDimensionsViewModel,
  OrderFurniturePartLineViewModel,
  PageInfo,
  PartDefinitionOrdersConnection,
  PartDefinitionsOrderViewModel,
} from '../../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../../shared/helpers/name-of.helper';

@Injectable({
  providedIn: 'root',
})
export class GetPartDefinitionOrdersGQL extends Query<GraphQLResponse<PartDefinitionsOrderViewModel[]>> {
  override document = gql`
    query partDefinitionOrders(
      $after: String
      $before: String
      $first: Int
      $last: Int
      $where: PartDefinitionsOrderViewModelFilterInput
      $order: [PartDefinitionsOrderViewModelSortInput!]
    ) {
      result: ${nameof<IntegratorQuery>('partDefinitionOrders')}(
        after: $after
        before: $before
        first: $first
        last: $last
        where: $where
        order: $order
      ) {
        ${nameof<PartDefinitionOrdersConnection>('nodes')} {
          ${nameof<PartDefinitionsOrderViewModel>('id')}
          ${nameof<PartDefinitionsOrderViewModel>('number')}
          ${nameof<PartDefinitionsOrderViewModel>('createdAt')}
          ${nameof<PartDefinitionsOrderViewModel>('createdBy')}
          ${nameof<PartDefinitionsOrderViewModel>('modifiedAt')}
          ${nameof<PartDefinitionsOrderViewModel>('modifiedBy')}
          ${nameof<PartDefinitionsOrderViewModel>('status')}
          ${nameof<PartDefinitionsOrderViewModel>('supplierId')}
          ${nameof<PartDefinitionsOrderViewModel>('tenantId')}

          ${nameof<PartDefinitionsOrderViewModel>('furnitureLines')} {
            ${nameof<OrderFurnitureLineViewModel>('id')}
            ${nameof<OrderFurnitureLineViewModel>('furnitureCode')}
            ${nameof<OrderFurnitureLineViewModel>('furnitureVersion')}
            ${nameof<OrderFurnitureLineViewModel>('quantityOrdered')}
            ${nameof<OrderFurnitureLineViewModel>('orderId')}
            ${nameof<OrderFurnitureLineViewModel>('tenantId')}

            ${nameof<OrderFurnitureLineViewModel>('partLines')} {
              ${nameof<OrderFurniturePartLineViewModel>('id')}
              ${nameof<OrderFurniturePartLineViewModel>('name')}
              ${nameof<OrderFurniturePartLineViewModel>('quantity')}
              ${nameof<OrderFurniturePartLineViewModel>('additionalInfo')}
              ${nameof<OrderFurniturePartLineViewModel>('orderFurnitureLineId')}

              ${nameof<OrderFurniturePartLineViewModel>('dimensions')} {
                ${nameof<OrderFurniturePartLineDimensionsViewModel>('lengthMm')}
                ${nameof<OrderFurniturePartLineDimensionsViewModel>('widthMm')}
                ${nameof<OrderFurniturePartLineDimensionsViewModel>('thicknessMm')}
                ${nameof<OrderFurniturePartLineDimensionsViewModel>('lengthEdgeBandingType')}
                ${nameof<OrderFurniturePartLineDimensionsViewModel>('widthEdgeBandingType')}
              }
            }
          }
        }
        ${nameof<PartDefinitionOrdersConnection>('totalCount')}
        ${nameof<PartDefinitionOrdersConnection>('pageInfo')} {
          ${nameof<PageInfo>('startCursor')}
          ${nameof<PageInfo>('endCursor')}
          ${nameof<PageInfo>('hasNextPage')}
          ${nameof<PageInfo>('hasPreviousPage')}
        }
      }
    }
  `;
}
