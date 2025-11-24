import { IntegrationWithType } from '../models/integration.model';

export interface IntegrationsStateModel {
  integrations: IntegrationWithType[];
  loading: boolean;
}
