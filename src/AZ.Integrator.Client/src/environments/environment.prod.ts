import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}`,
  apiEndpoint: 'https://SERWER:6010/api',
  graphqlEndpoint: 'https://SERWER:6010/api/graphql',
  keycloakEndpoint: 'https://SERWER:9080',
  defaultPageSize: 10,

  // Tenants
  allegroLoginEndpoint: 'https://SERWER:6010/api/auth/login-allegro?tenantId=',

  allegroAzTeamTenantId: 'allegro-az-team',
  allegroMebleplTenantId: 'allegro-meblepl',
  allegroMyTestTenantId: 'allegro-my-test',
  erliAzTeamTenantId: 'erli-az-team',

  showMyTestAccounts: false,

  // Stocks
  stockWarningThreshold: 20,
};
