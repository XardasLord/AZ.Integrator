import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}-dev`,
  apiEndpoint: 'http://localhost:6010/api',
  graphqlEndpoint: 'http://localhost:6010/api/graphql',
  defaultPageSize: 10,

  // Tenants
  allegroLoginEndpoint: 'http://localhost:6010/api/auth/login-allegro?tenantId=',

  allegroAzTeamTenantId: 'allegro-az-team',
  allegroMebleplTenantId: 'allegro-meblepl',
  allegroMyTestTenantId: 'allegro-my-test',
  erliAzTeamTenantId: 'erli-az-team',

  showMyTestAccounts: true,

  // Stocks
  stockWarningThreshold: 20,
};
