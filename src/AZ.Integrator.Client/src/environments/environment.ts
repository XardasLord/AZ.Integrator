import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}-dev`,
  apiEndpoint: 'http://localhost:6010/api',
  graphqlEndpoint: 'http://localhost:6010/api/graphql',
  defaultPageSize: 10,

  // Tenants
  allegroLoginEndpoint: 'http://localhost:6010/api/auth/login-allegro?tenantId=',
  erliLoginEndpoint: 'http://localhost:6010/api/auth/login-erli?tenantId=',

  allegroAzTeamTenantId: 'az-team',
  allegroMebleplTenantId: 'meblepl',
  allegroMyTestTenantId: 'my-test',
  erliAzTeamTenantId: 'az-team',

  showMyTestAccounts: true,
};
