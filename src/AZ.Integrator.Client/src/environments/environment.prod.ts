import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}`,
  apiEndpoint: 'http://SERWER:6010/api',
  graphqlEndpoint: 'http://SERWER:6010/api/graphql',
  defaultPageSize: 10,

  // Tenants
  allegroLoginEndpoint: 'http://SERWER:6010/api/auth/login-allegro?tenantId=',
  erliLoginEndpoint: 'http://SERWER:6010/api/auth/login-erli?tenantId=',

  allegroAzTeamTenantId: 'az-team',
  allegroMebleplTenantId: 'meblepl',
  allegroMyTestTenantId: 'my-test',
  erliAzTeamTenantId: 'az-team',

  showMyTestAccounts: false,
};
