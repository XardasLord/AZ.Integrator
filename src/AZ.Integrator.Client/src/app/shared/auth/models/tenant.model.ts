export class Tenant {
  public tenantId!: string;
  public displayName!: string;
  public subtitle?: string;
  public authorizationProvider!: AuthorizationProvider;
}

export class TenantGroup {
  public groupName!: string;
  public tenants!: Tenant[];
}

export enum AuthorizationProvider {
  Allegro = 1,
  Erli = 2,
  Shopify = 3,
}
