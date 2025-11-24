export enum IntegrationType {
  Allegro = 'Allegro',
  Erli = 'Erli',
  Shopify = 'Shopify',
  Fakturownia = 'Fakturownia',
  Inpost = 'Inpost',
  Dpd = 'Dpd',
}

export const IntegrationTypeLabels: Record<IntegrationType, string> = {
  [IntegrationType.Allegro]: 'Allegro',
  [IntegrationType.Erli]: 'Erli',
  [IntegrationType.Shopify]: 'Shopify',
  [IntegrationType.Fakturownia]: 'Fakturownia',
  [IntegrationType.Inpost]: 'InPost (ShipX)',
  [IntegrationType.Dpd]: 'DPD',
};
