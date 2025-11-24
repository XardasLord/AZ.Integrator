import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { GraphQLResponse } from '../../../shared/graphql/graphql.response';
import {
  AllegroIntegrationViewModel,
  AllegroIntegrationsConnection,
  IntegratorQuery,
} from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
@Injectable({
  providedIn: 'root',
})
export class GetAllegroIntegrationsGQL extends Query<GraphQLResponse<AllegroIntegrationViewModel[]>> {
  override document = gql`
    query allegroIntegrations {
      result: ${nameof<IntegratorQuery>('allegroIntegrations')} {
        ${nameof<AllegroIntegrationsConnection>('nodes')} {
          ${nameof<AllegroIntegrationViewModel>('sourceSystemId')}
          ${nameof<AllegroIntegrationViewModel>('displayName')}
          ${nameof<AllegroIntegrationViewModel>('isEnabled')}
          ${nameof<AllegroIntegrationViewModel>('clientId')}
          ${nameof<AllegroIntegrationViewModel>('redirectUri')}
          ${nameof<AllegroIntegrationViewModel>('createdAt')}
          ${nameof<AllegroIntegrationViewModel>('updatedAt')}
          ${nameof<AllegroIntegrationViewModel>('tenantId')}
        }
        totalCount
      }
    }
  `;
}

