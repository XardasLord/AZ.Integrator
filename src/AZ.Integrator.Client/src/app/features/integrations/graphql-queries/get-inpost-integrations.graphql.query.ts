import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { GraphQLResponse } from '../../../shared/graphql/graphql.response';
import {
  InpostIntegrationViewModel,
  InpostIntegrationsConnection,
  IntegratorQuery,
} from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
@Injectable({
  providedIn: 'root',
})
export class GetInpostIntegrationsGQL extends Query<GraphQLResponse<InpostIntegrationViewModel[]>> {
  override document = gql`
    query inpostIntegrations {
      result: ${nameof<IntegratorQuery>('inpostIntegrations')} {
        ${nameof<InpostIntegrationsConnection>('nodes')} {
          ${nameof<InpostIntegrationViewModel>('displayName')}
          ${nameof<InpostIntegrationViewModel>('isEnabled')}
          ${nameof<InpostIntegrationViewModel>('organizationId')}
          ${nameof<InpostIntegrationViewModel>('accessToken')}
          ${nameof<InpostIntegrationViewModel>('senderName')}
          ${nameof<InpostIntegrationViewModel>('senderCompanyName')}
          ${nameof<InpostIntegrationViewModel>('senderEmail')}
          ${nameof<InpostIntegrationViewModel>('senderPhone')}
          ${nameof<InpostIntegrationViewModel>('senderAddressStreet')}
          ${nameof<InpostIntegrationViewModel>('senderAddressCity')}
          ${nameof<InpostIntegrationViewModel>('senderAddressPostCode')}
          ${nameof<InpostIntegrationViewModel>('createdAt')}
          ${nameof<InpostIntegrationViewModel>('updatedAt')}
          ${nameof<InpostIntegrationViewModel>('tenantId')}
        }
        totalCount
      }
    }
  `;
}

