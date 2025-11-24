import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { GraphQLResponse } from '../../../shared/graphql/graphql.response';
import {
  ErliIntegrationViewModel,
  ErliIntegrationsConnection,
  IntegratorQuery,
} from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
@Injectable({
  providedIn: 'root',
})
export class GetErliIntegrationsGQL extends Query<GraphQLResponse<ErliIntegrationViewModel[]>> {
  override document = gql`
    query erliIntegrations {
      result: ${nameof<IntegratorQuery>('erliIntegrations')} {
        ${nameof<ErliIntegrationsConnection>('nodes')} {
          ${nameof<ErliIntegrationViewModel>('sourceSystemId')}
          ${nameof<ErliIntegrationViewModel>('displayName')}
          ${nameof<ErliIntegrationViewModel>('isEnabled')}
          ${nameof<ErliIntegrationViewModel>('apiKey')}
          ${nameof<ErliIntegrationViewModel>('createdAt')}
          ${nameof<ErliIntegrationViewModel>('updatedAt')}
          ${nameof<ErliIntegrationViewModel>('tenantId')}
        }
        totalCount
      }
    }
  `;
}

