export enum IntegrationType {
  Allegro = 'Allegro',
  Erli = 'Erli',
  Shopify = 'Shopify',
  Fakturownia = 'Fakturownia',
  Inpost = 'Inpost',
  Dpd = 'Dpd',
}

export enum IntegrationCategory {
  Marketplace = 'Marketplace',
  Courier = 'Kurierzy',
  Accounting = 'Księgowość',
}

export const IntegrationTypeLabels: Record<IntegrationType, string> = {
  [IntegrationType.Allegro]: 'Allegro',
  [IntegrationType.Erli]: 'Erli',
  [IntegrationType.Shopify]: 'Shopify',
  [IntegrationType.Fakturownia]: 'Fakturownia',
  [IntegrationType.Inpost]: 'InPost (ShipX)',
  [IntegrationType.Dpd]: 'DPD',
};

export const IntegrationTypeLogos: Record<IntegrationType, string> = {
  [IntegrationType.Allegro]: 'assets/logo/allegro.svg',
  [IntegrationType.Erli]: 'assets/logo/erli.svg',
  [IntegrationType.Shopify]: 'assets/logo/shopify.svg',
  [IntegrationType.Fakturownia]: 'assets/logo/fakturownia.svg',
  [IntegrationType.Inpost]: 'assets/logo/inpost.svg',
  [IntegrationType.Dpd]: 'assets/logo/dpd.svg',
};

export const IntegrationTypeCategories: Record<IntegrationType, IntegrationCategory> = {
  [IntegrationType.Allegro]: IntegrationCategory.Marketplace,
  [IntegrationType.Erli]: IntegrationCategory.Marketplace,
  [IntegrationType.Shopify]: IntegrationCategory.Marketplace,
  [IntegrationType.Fakturownia]: IntegrationCategory.Accounting,
  [IntegrationType.Inpost]: IntegrationCategory.Courier,
  [IntegrationType.Dpd]: IntegrationCategory.Courier,
};
