import { Tenant } from '../auth/models/tenant.model';

const prefix = '[Tenant]';

export class ChangeTenant {
  static readonly type = `${prefix} ${ChangeTenant.name}`;

  constructor(public tenant: Tenant) {}
}
