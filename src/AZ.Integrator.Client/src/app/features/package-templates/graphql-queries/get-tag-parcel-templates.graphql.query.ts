import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { GraphQLResponse } from 'src/app/shared/graphql/graphql.response';
import {
  IntegratorQuery,
  PageInfo,
  TagParcelTemplatesConnection,
  TagParcelTemplateViewModel,
  TagParcelViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';

@Injectable({
  providedIn: 'root',
})
export class GetTagParcelTemplatesGQL extends Query<GraphQLResponse<TagParcelTemplateViewModel[]>> {
  override document = gql`
    query tagParcelTemplates(
      $after: String, $before: String, $first: Int, $last: Int, $where: TagParcelTemplateViewModelFilterInput
    ) {
      result: ${nameof<IntegratorQuery>('tagParcelTemplates')}(
        after: $after, before: $before, first: $first, last: $last, where: $where
      ) {
            ${nameof<TagParcelTemplatesConnection>('nodes')} {
              ${nameof<TagParcelTemplateViewModel>('tag')}
              ${nameof<TagParcelTemplateViewModel>('parcels')} {
                ${nameof<TagParcelViewModel>('length')}
                ${nameof<TagParcelViewModel>('width')}
                ${nameof<TagParcelViewModel>('height')}
                ${nameof<TagParcelViewModel>('weight')}
              }
            }
            ${nameof<TagParcelTemplatesConnection>('totalCount')}
            ${nameof<TagParcelTemplatesConnection>('pageInfo')} {
              ${nameof<PageInfo>('startCursor')}
              ${nameof<PageInfo>('endCursor')}
              ${nameof<PageInfo>('hasNextPage')}
              ${nameof<PageInfo>('hasPreviousPage')}
            }
        }
      }
    `;
}
