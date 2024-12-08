export class Tenant {
  public tenantId!: string;
  public displayName!: string;
  public authorizationProvider!: AuthorizationProvider;
  public isTestAccount!: boolean;
}

export enum AuthorizationProvider {
  Allegro,
  Erli,
}
