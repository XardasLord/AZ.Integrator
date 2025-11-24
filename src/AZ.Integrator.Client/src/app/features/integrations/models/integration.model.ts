import {
  AllegroIntegrationViewModel,
  ErliIntegrationViewModel,
  FakturowniaIntegrationViewModel,
  InpostIntegrationViewModel,
  ShopifyIntegrationViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';
import { IntegrationType } from './integration-type.enum';

export type Integration =
  | AllegroIntegrationViewModel
  | ErliIntegrationViewModel
  | ShopifyIntegrationViewModel
  | FakturowniaIntegrationViewModel
  | InpostIntegrationViewModel
  | DpdIntegrationPlaceholder;

export interface DpdIntegrationPlaceholder {
  __typename?: 'DpdIntegrationViewModel';
  sourceSystemId: string;
  displayName: string;
  isEnabled: boolean;
  login: number;
  masterFid: number;
  senderName: string;
  senderCompany: string;
  senderEmail: string;
  senderPhone: string;
  senderAddress: string;
  senderAddressCity: string;
  senderAddressPostCode: string;
  senderAddressCountryCode: string;
  createdAt: string;
  updatedAt: string;
  tenantId: string;
}

export interface IntegrationWithType {
  type: IntegrationType;
  integration: Integration;
}

export function getIntegrationType(integration: Integration): IntegrationType {
  switch (integration.__typename) {
    case 'AllegroIntegrationViewModel':
      return IntegrationType.Allegro;
    case 'ErliIntegrationViewModel':
      return IntegrationType.Erli;
    case 'ShopifyIntegrationViewModel':
      return IntegrationType.Shopify;
    case 'FakturowniaIntegrationViewModel':
      return IntegrationType.Fakturownia;
    case 'InpostIntegrationViewModel':
      return IntegrationType.Inpost;
    case 'DpdIntegrationViewModel':
      return IntegrationType.Dpd;
    default:
      throw new Error('Unknown integration type');
  }
}
