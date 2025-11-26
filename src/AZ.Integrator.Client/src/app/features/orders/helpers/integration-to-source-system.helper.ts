import { IntegrationWithType } from '../../integrations/models/integration.model';
import { IntegrationType } from '../../integrations/models/integration-type.enum';
import {
  AuthorizationProvider,
  SourceSystem,
  SourceSystemGroup,
} from '../../../shared/auth/models/source-system.model';
import {
  AllegroIntegrationViewModel,
  ErliIntegrationViewModel,
  ShopifyIntegrationViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';

type MarketplaceIntegration = AllegroIntegrationViewModel | ErliIntegrationViewModel | ShopifyIntegrationViewModel;

export class IntegrationToSourceSystemHelper {
  static convertToSourceSystemGroups(integrations: IntegrationWithType[]): SourceSystemGroup[] {
    const allegroIntegrations = integrations.filter(i => i.type === IntegrationType.Allegro);
    const erliIntegrations = integrations.filter(i => i.type === IntegrationType.Erli);
    const shopifyIntegrations = integrations.filter(i => i.type === IntegrationType.Shopify);

    const groups: SourceSystemGroup[] = [];

    if (allegroIntegrations.length > 0) {
      groups.push({
        groupName: 'ALLEGRO',
        sourceSystems: allegroIntegrations.map(i => this.convertToSourceSystem(i)),
      });
    }

    if (erliIntegrations.length > 0) {
      groups.push({
        groupName: 'ERLI',
        sourceSystems: erliIntegrations.map(i => this.convertToSourceSystem(i)),
      });
    }

    if (shopifyIntegrations.length > 0) {
      groups.push({
        groupName: 'SHOPIFY',
        sourceSystems: shopifyIntegrations.map(i => this.convertToSourceSystem(i)),
      });
    }

    return groups;
  }

  private static convertToSourceSystem(integration: IntegrationWithType): SourceSystem {
    if (!this.isMarketplaceIntegration(integration.integration)) {
      throw new Error('Integration must be a marketplace integration (Allegro, Erli, or Shopify)');
    }

    const sourceSystem = new SourceSystem();
    sourceSystem.sourceSystemId = integration.integration.sourceSystemId;
    sourceSystem.displayName = integration.integration.displayName;
    sourceSystem.authorizationProvider = this.getAuthorizationProvider(integration.type);
    sourceSystem.subtitle = this.getSubtitle(integration);

    return sourceSystem;
  }

  private static isMarketplaceIntegration(integration: unknown): integration is MarketplaceIntegration {
    return (
      (integration as MarketplaceIntegration).__typename === 'AllegroIntegrationViewModel' ||
      (integration as MarketplaceIntegration).__typename === 'ErliIntegrationViewModel' ||
      (integration as MarketplaceIntegration).__typename === 'ShopifyIntegrationViewModel'
    );
  }

  private static getAuthorizationProvider(integrationType: IntegrationType): AuthorizationProvider {
    switch (integrationType) {
      case IntegrationType.Allegro:
        return AuthorizationProvider.Allegro;
      case IntegrationType.Erli:
        return AuthorizationProvider.Erli;
      case IntegrationType.Shopify:
        return AuthorizationProvider.Shopify;
      default:
        throw new Error(`Unsupported integration type: ${integrationType}`);
    }
  }

  private static getSubtitle(integration: IntegrationWithType): string {
    switch (integration.type) {
      case IntegrationType.Allegro:
        return 'ALLEGRO';
      case IntegrationType.Erli:
        return 'ERLI';
      case IntegrationType.Shopify: {
        const shopifyIntegration = integration.integration as ShopifyIntegrationViewModel;
        return shopifyIntegration.apiUrl || 'SHOPIFY';
      }
      default:
        return '';
    }
  }
}
