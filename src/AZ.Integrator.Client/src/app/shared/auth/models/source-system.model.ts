export class SourceSystem {
  public sourceSystemId!: string;
  public displayName!: string;
  public subtitle?: string;
  public authorizationProvider!: AuthorizationProvider;
}

export class SourceSystemGroup {
  public groupName!: string;
  public sourceSystems!: SourceSystem[];
}

export enum AuthorizationProvider {
  Allegro = 1,
  Erli = 2,
  Shopify = 3,
}
