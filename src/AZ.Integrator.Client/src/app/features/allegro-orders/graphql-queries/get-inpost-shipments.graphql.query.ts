import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { InpostShipmentViewModel, IntegratorQuery } from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';

@Injectable({
  providedIn: 'root',
})
export class GetInpostShipmentsGQL extends Query<GraphQLResponseWithoutPaginationVo<InpostShipmentViewModel[]>> {
  override document = gql`
    query inpostShipments(
      $where: InpostShipmentViewModelFilterInput
    ) {
      result: ${nameof<IntegratorQuery>('inpostShipments')}(
        where: $where
      ) {
            ${nameof<InpostShipmentViewModel>('inpostShipmentNumber')}
            ${nameof<InpostShipmentViewModel>('allegroOrderNumber')}
            ${nameof<InpostShipmentViewModel>('createdAt')}
        }
      }
      `;
}
