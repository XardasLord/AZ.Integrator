import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { GraphQLResponse } from 'src/app/shared/graphql/graphql.response';
import {
  DimensionsViewModel,
  FurnitureModelsConnection,
  FurnitureModelViewModel,
  IntegratorQuery,
  PageInfo,
  PartDefinitionViewModel,
} from '../../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../../shared/helpers/name-of.helper';

@Injectable({
  providedIn: 'root',
})
export class GetFurnitureDefinitionsGQL extends Query<GraphQLResponse<FurnitureModelViewModel[]>> {
  override document = gql`
    query furnitureModels(
      $after: String
      $before: String
      $first: Int
      $last: Int
      $where: FurnitureModelViewModelFilterInput
      $order: [FurnitureModelViewModelSortInput!]
    ) {
      result: ${nameof<IntegratorQuery>('furnitureModels')}(
        after: $after
        before: $before
        first: $first
        last: $last
        where: $where
        order: $order
      ) {

        ${nameof<FurnitureModelsConnection>('nodes')} {
          ${nameof<FurnitureModelViewModel>('furnitureCode')}
          ${nameof<FurnitureModelViewModel>('version')}
          ${nameof<FurnitureModelViewModel>('isDeleted')}
          ${nameof<FurnitureModelViewModel>('deletedAt')}
          ${nameof<FurnitureModelViewModel>('createdAt')}
          ${nameof<FurnitureModelViewModel>('createdBy')}
          ${nameof<FurnitureModelViewModel>('modifiedAt')}
          ${nameof<FurnitureModelViewModel>('modifiedBy')}

          ${nameof<FurnitureModelViewModel>('partDefinitions')} {
            ${nameof<PartDefinitionViewModel>('id')}
            ${nameof<PartDefinitionViewModel>('furnitureCode')}
            ${nameof<PartDefinitionViewModel>('name')}
            ${nameof<PartDefinitionViewModel>('dimensions')} {
              ${nameof<DimensionsViewModel>('lengthMm')}
              ${nameof<DimensionsViewModel>('widthMm')}
              ${nameof<DimensionsViewModel>('thicknessMm')}
            }
            ${nameof<PartDefinitionViewModel>('color')}
            ${nameof<PartDefinitionViewModel>('additionalInfo')}
          }
        }
        ${nameof<FurnitureModelsConnection>('totalCount')}
        ${nameof<FurnitureModelsConnection>('pageInfo')} {
          ${nameof<PageInfo>('startCursor')}
          ${nameof<PageInfo>('endCursor')}
          ${nameof<PageInfo>('hasNextPage')}
          ${nameof<PageInfo>('hasPreviousPage')}
        }
      }
    }
  `;
}
