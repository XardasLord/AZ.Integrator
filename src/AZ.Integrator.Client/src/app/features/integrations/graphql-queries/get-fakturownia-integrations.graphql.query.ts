import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { GraphQLResponse } from '../../../shared/graphql/graphql.response';
import {
  FakturowniaIntegrationViewModel,
  FakturowniaIntegrationsConnection,
  IntegratorQuery,
} from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
@Injectable({
  providedIn: 'root',
})
export class GetFakturowniaIntegrationsGQL extends Query<GraphQLResponse<FakturowniaIntegrationViewModel[]>> {
  override document = gql`
    query fakturowniaIntegrations {
      result: ${nameof<IntegratorQuery>('fakturowniaIntegrations')} {
        ${nameof<FakturowniaIntegrationsConnection>('nodes')} {
          ${nameof<FakturowniaIntegrationViewModel>('sourceSystemId')}
          ${nameof<FakturowniaIntegrationViewModel>('displayName')}
          ${nameof<FakturowniaIntegrationViewModel>('isEnabled')}
          ${nameof<FakturowniaIntegrationViewModel>('apiKey')}
          ${nameof<FakturowniaIntegrationViewModel>('apiUrl')}
          ${nameof<FakturowniaIntegrationViewModel>('createdAt')}
          ${nameof<FakturowniaIntegrationViewModel>('updatedAt')}
          ${nameof<FakturowniaIntegrationViewModel>('tenantId')}
        }
        totalCount
      }
    }
  `;
}

