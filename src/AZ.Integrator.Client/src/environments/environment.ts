import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}-dev`,
  apiEndpoint: 'http://192.168.0.185:6010/api',
  graphqlEndpoint: 'http://192.168.0.185:6010/api/graphql',
  keycloakEndpoint: 'http://192.168.0.185:9080',
  defaultPageSize: 10,

  // Tenants
  allegroLoginEndpoint: 'http://192.168.0.185:6010/api/auth/login-allegro?tenantId=',

  allegroAzTeamTenantId: 'allegro-az-team',
  allegroMebleplTenantId: 'allegro-meblepl',
  allegroMyTestTenantId: 'allegro-my-test',
  erliAzTeamTenantId: 'erli-az-team',

  showMyTestAccounts: true,

  // Stocks
  stockWarningThreshold: 20,
};
