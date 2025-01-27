import { Tenant } from '../auth/models/tenant.model';

export interface TenantStateModel {
  tenant: Tenant | null;
}
