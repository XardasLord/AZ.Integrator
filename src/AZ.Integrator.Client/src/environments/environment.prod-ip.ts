import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}`,
  apiEndpoint: 'https://192.168.1.250:6011/api',
  graphqlEndpoint: 'https://192.168.1.250:6011/api/graphql',
  keycloakEndpoint: 'https://192.168.1.250:9080',
  defaultPageSize: 10,

  // Tenants
  allegroLoginEndpoint: 'https://192.168.1.250:6011/api/auth/login-allegro?tenantId=',

  allegroAzTeamTenantId: 'allegro-az-team',
  allegroMebleplTenantId: 'allegro-meblepl',
  allegroMyTestTenantId: 'allegro-my-test',
  erliAzTeamTenantId: 'erli-az-team',
  shopifyUmeblovaneTenantId: 'shopify-umeblovane',

  showMyTestAccounts: false,
};
