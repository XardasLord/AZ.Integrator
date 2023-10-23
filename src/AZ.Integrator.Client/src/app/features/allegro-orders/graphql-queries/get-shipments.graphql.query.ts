import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { IntegratorQuery, ShipmentViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';

@Injectable({
  providedIn: 'root',
})
export class GetShipmentsGQL extends Query<GraphQLResponseWithoutPaginationVo<ShipmentViewModel[]>> {
  override document = gql`
    query shipments(
      $where: ShipmentViewModelFilterInput
    ) {
      result: ${nameof<IntegratorQuery>('shipments')}(
        where: $where
      ) {
            ${nameof<ShipmentViewModel>('shipmentNumber')}
            ${nameof<ShipmentViewModel>('allegroOrderNumber')}
            ${nameof<ShipmentViewModel>('trackingNumber')}
            ${nameof<ShipmentViewModel>('shipmentProvider')}
            ${nameof<ShipmentViewModel>('createdAt')}
        }
      }
      `;
}
