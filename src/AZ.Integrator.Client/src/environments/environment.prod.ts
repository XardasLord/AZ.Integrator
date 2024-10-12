import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}`,
  apiEndpoint: 'http://SERWER:6010/api',
  graphqlEndpoint: 'http://SERWER:6010/api/graphql',
  defaultPageSize: 10,
  allegroLoginEndpointForAzTeamTenant: 'http://SERWER:6010/auth/login-allegro?tenantId=az-team',
  allegroLoginEndpointForMebleplTenant: 'http://SERWER:6010/auth/login-allegro?tenantId=meblepl',
  allegroLoginEndpointForMyTestTenant: 'http://SERWER:6010/auth/login-allegro?tenantId=my-test',

  showMyTestAllegroAccount: false,
};
