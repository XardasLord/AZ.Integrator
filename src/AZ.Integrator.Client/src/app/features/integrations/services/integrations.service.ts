import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { RemoteServiceBase } from '../../../shared/services/remote-service.base';
import { GetAllegroIntegrationsGQL } from '../graphql-queries/get-allegro-integrations.graphql.query';
import { GetErliIntegrationsGQL } from '../graphql-queries/get-erli-integrations.graphql.query';
import { GetShopifyIntegrationsGQL } from '../graphql-queries/get-shopify-integrations.graphql.query';
import { GetFakturowniaIntegrationsGQL } from '../graphql-queries/get-fakturownia-integrations.graphql.query';
import { GetInpostIntegrationsGQL } from '../graphql-queries/get-inpost-integrations.graphql.query';
import {
  AddDpdIntegrationCommand,
  AddErliIntegrationCommand,
  AddFakturowniaIntegrationCommand,
  AddInpostIntegrationCommand,
  AddShopifyIntegrationCommand,
  UpdateIntegrationCommand,
} from '../models/integration-commands.model';
import { IntegrationWithType } from '../models/integration.model';
import { IntegrationType } from '../models/integration-type.enum';

@Injectable({
  providedIn: 'root',
})
export class IntegrationsService extends RemoteServiceBase {
  private getAllegroIntegrationsGql = inject(GetAllegroIntegrationsGQL);
  private getErliIntegrationsGql = inject(GetErliIntegrationsGQL);
  private getShopifyIntegrationsGql = inject(GetShopifyIntegrationsGQL);
  private getFakturowniaIntegrationsGql = inject(GetFakturowniaIntegrationsGQL);
  private getInpostIntegrationsGql = inject(GetInpostIntegrationsGQL);

  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);
    super(httpClient);
  }

  getAllIntegrations(): Observable<IntegrationWithType[]> {
    return forkJoin({
      allegro: this.getAllegroIntegrationsGql.fetch().pipe(map(x => x.data?.result?.nodes || [])),
      erli: this.getErliIntegrationsGql.fetch().pipe(map(x => x.data?.result?.nodes || [])),
      shopify: this.getShopifyIntegrationsGql.fetch().pipe(map(x => x.data?.result?.nodes || [])),
      fakturownia: this.getFakturowniaIntegrationsGql.fetch().pipe(map(x => x.data?.result?.nodes || [])),
      inpost: this.getInpostIntegrationsGql.fetch().pipe(map(x => x.data?.result?.nodes || [])),
    }).pipe(
      map(results => {
        const integrations: IntegrationWithType[] = [];

        if (results.allegro) {
          results.allegro.forEach(integration => {
            integrations.push({
              type: IntegrationType.Allegro,
              integration,
            });
          });
        }

        if (results.erli) {
          results.erli.forEach(integration => {
            integrations.push({
              type: IntegrationType.Erli,
              integration,
            });
          });
        }

        if (results.shopify) {
          results.shopify.forEach(integration => {
            integrations.push({
              type: IntegrationType.Shopify,
              integration,
            });
          });
        }

        if (results.fakturownia) {
          results.fakturownia.forEach(integration => {
            integrations.push({
              type: IntegrationType.Fakturownia,
              integration,
            });
          });
        }

        if (results.inpost) {
          results.inpost.forEach(integration => {
            integrations.push({
              type: IntegrationType.Inpost,
              integration,
            });
          });
        }

        // DPD placeholder - nie ma jeszcze zapytania GraphQL
        // integrations.push(...dpdIntegrations);

        return integrations;
      })
    );
  }

  connectAllegro(): Observable<void> {
    // Redirect do API dla autoryzacji OAuth
    window.location.href = `${this.apiUrl}/integrations/allegro`;
    return new Observable();
  }

  addErliIntegration(command: AddErliIntegrationCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/integrations/erli`, command);
  }

  addShopifyIntegration(command: AddShopifyIntegrationCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/integrations/shopify`, command);
  }

  addFakturowniaIntegration(command: AddFakturowniaIntegrationCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/integrations/fakturownia`, command);
  }

  addInpostIntegration(command: AddInpostIntegrationCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/integrations/inpost`, command);
  }

  addDpdIntegration(command: AddDpdIntegrationCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/integrations/dpd`, command);
  }

  updateIntegrationStatus(type: IntegrationType, sourceSystemId: string, isEnabled: boolean): Observable<void> {
    const command: UpdateIntegrationCommand = { sourceSystemId, isEnabled };
    const endpoint = this.getEndpointForType(type);
    return this.httpClient.put<void>(`${this.apiUrl}/integrations/${endpoint}/${sourceSystemId}`, command);
  }

  deleteIntegration(type: IntegrationType, sourceSystemId: string): Observable<void> {
    const endpoint = this.getEndpointForType(type);
    return this.httpClient.delete<void>(`${this.apiUrl}/integrations/${endpoint}/${sourceSystemId}`);
  }

  testConnection(type: IntegrationType, sourceSystemId: string): Observable<{ isValid: boolean; message?: string }> {
    const endpoint = this.getEndpointForType(type);
    return this.httpClient.get<{ isValid: boolean; message?: string }>(
      `${this.apiUrl}/integrations/${endpoint}/${sourceSystemId}/test`
    );
  }

  updateErliIntegration(sourceSystemId: string, command: AddErliIntegrationCommand): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/integrations/erli/${sourceSystemId}`, command);
  }

  updateShopifyIntegration(sourceSystemId: string, command: AddShopifyIntegrationCommand): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/integrations/shopify/${sourceSystemId}`, command);
  }

  updateFakturowniaIntegration(sourceSystemId: string, command: AddFakturowniaIntegrationCommand): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/integrations/fakturownia/${sourceSystemId}`, command);
  }

  updateInpostIntegration(organizationId: number, command: AddInpostIntegrationCommand): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/integrations/inpost/${organizationId}`, command);
  }

  updateDpdIntegration(login: number, command: AddDpdIntegrationCommand): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/integrations/dpd/${login}`, command);
  }

  private getEndpointForType(type: IntegrationType): string {
    switch (type) {
      case IntegrationType.Allegro:
        return 'allegro';
      case IntegrationType.Erli:
        return 'erli';
      case IntegrationType.Shopify:
        return 'shopify';
      case IntegrationType.Fakturownia:
        return 'fakturownia';
      case IntegrationType.Inpost:
        return 'inpost';
      case IntegrationType.Dpd:
        return 'dpd';
      default:
        throw new Error('Unknown integration type');
    }
  }
}
