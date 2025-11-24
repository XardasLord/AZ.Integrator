import { Injectable } from '@angular/core';
import { gql, Query } from 'apollo-angular';
import { GraphQLResponse } from '../../../shared/graphql/graphql.response';
import {
  ShopifyIntegrationViewModel,
  ShopifyIntegrationsConnection,
  IntegratorQuery,
} from '../../../shared/graphql/graphql-integrator.schema';
import { nameof } from '../../../shared/helpers/name-of.helper';
@Injectable({
  providedIn: 'root',
})
export class GetShopifyIntegrationsGQL extends Query<GraphQLResponse<ShopifyIntegrationViewModel[]>> {
  override document = gql`
    query shopifyIntegrations {
      result: ${nameof<IntegratorQuery>('shopifyIntegrations')} {
        ${nameof<ShopifyIntegrationsConnection>('nodes')} {
          ${nameof<ShopifyIntegrationViewModel>('sourceSystemId')}
          ${nameof<ShopifyIntegrationViewModel>('displayName')}
          ${nameof<ShopifyIntegrationViewModel>('isEnabled')}
          ${nameof<ShopifyIntegrationViewModel>('apiUrl')}
          ${nameof<ShopifyIntegrationViewModel>('adminApiToken')}
          ${nameof<ShopifyIntegrationViewModel>('createdAt')}
          ${nameof<ShopifyIntegrationViewModel>('updatedAt')}
          ${nameof<ShopifyIntegrationViewModel>('tenantId')}
        }
        totalCount
      }
    }
  `;
}

