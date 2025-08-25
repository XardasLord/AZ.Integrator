import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}-dev`,
  apiEndpoint: 'https://192.168.0.185:6010/api',
  graphqlEndpoint: 'https://192.168.0.185:6010/api/graphql',
  keycloakEndpoint: 'https://192.168.0.185:9080',
  defaultPageSize: 10,

  // Tenants
  allegroLoginEndpoint: 'https://192.168.0.185:6010/api/auth/login-allegro?tenantId=',

  allegroAzTeamTenantId: 'allegro-az-team',
  allegroMebleplTenantId: 'allegro-meblepl',
  erliAzTeamTenantId: 'erli-az-team',
  shopifyUmeblovaneTenantId: 'shopify-umeblovane',
};
