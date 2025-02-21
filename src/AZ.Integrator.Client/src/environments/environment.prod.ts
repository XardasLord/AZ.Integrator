import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}`,
  apiEndpoint: 'http://SERWER:6010/api',
  graphqlEndpoint: 'http://SERWER:6010/api/graphql',
  keycloakEndpoint: 'http://SERWER:9080',
  defaultPageSize: 10,

  // Tenants
  allegroLoginEndpoint: 'http://SERWER:6010/api/auth/login-allegro?tenantId=',

  allegroAzTeamTenantId: 'allegro-az-team',
  allegroMebleplTenantId: 'allegro-meblepl',
  allegroMyTestTenantId: 'allegro-my-test',
  erliAzTeamTenantId: 'erli-az-team',

  showMyTestAccounts: false,

  // Stocks
  stockWarningThreshold: 20,
};
