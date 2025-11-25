import { IntegrationType } from '../models/integration-type.enum';
import { IntegrationWithType } from '../models/integration.model';

export class LoadIntegrations {
  static readonly type = '[Integrations] Load Integrations';
}

export class AddIntegration {
  static readonly type = '[Integrations] Add Integration';
  constructor(public integration: IntegrationWithType) {}
}

export class ToggleIntegrationStatus {
  static readonly type = '[Integrations] Toggle Integration Status';
  constructor(
    public integrationType: IntegrationType,
    public sourceSystemId: string,
    public isEnabled: boolean
  ) {}
}

export class DeleteIntegration {
  static readonly type = '[Integrations] Delete Integration';
  constructor(
    public integrationType: IntegrationType,
    public sourceSystemId: string
  ) {}
}

export class TestIntegrationConnection {
  static readonly type = '[Integrations] Test Integration Connection';
  constructor(
    public integrationType: IntegrationType,
    public sourceSystemId: string
  ) {}
}
