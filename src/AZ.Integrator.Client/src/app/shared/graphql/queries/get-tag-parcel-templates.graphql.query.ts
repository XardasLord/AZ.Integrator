import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { IntegratorQuery, TagParcelTemplateViewModel, TagParcelViewModel } from '../graphql-integrator.schema';
import { nameof } from '../../helpers/name-of.helper';
import { GraphQLResponseWithoutPaginationVo } from '../graphql.response';

@Injectable({
  providedIn: 'root',
})
export class GetTagParcelTemplatesGQL extends Query<GraphQLResponseWithoutPaginationVo<TagParcelTemplateViewModel[]>> {
  override document = gql`
    query tagParcelTemplates(
      $where: TagParcelTemplateViewModelFilterInput
    ) {
      result: ${nameof<IntegratorQuery>('tagParcelTemplates')}(
        where: $where
      ) {
            ${nameof<TagParcelTemplateViewModel>('tag')}
            ${nameof<TagParcelTemplateViewModel>('parcels')} {
              ${nameof<TagParcelViewModel>('length')}
              ${nameof<TagParcelViewModel>('width')}
              ${nameof<TagParcelViewModel>('height')}
              ${nameof<TagParcelViewModel>('weight')}
            }
        }
      }
    `;
}
