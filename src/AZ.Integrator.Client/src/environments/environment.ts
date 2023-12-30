import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}-dev`,
  apiEndpoint: 'http://localhost:6010/api',
  graphqlEndpoint: 'http://localhost:6010/api/graphql',
  defaultPageSize: 10,
  allegroLoginEndpointForAzTeamTenant: 'http://localhost:6010/auth/login-allegro?tenantId=az-team',
  allegroLoginEndpointForMebleplTenant: 'http://localhost:6010/auth/login-allegro?tenantId=meblepl',
  allegroLoginEndpointForMyTestTenant: 'http://localhost:6010/auth/login-allegro?tenantId=my-test',

  showMyTestAllegroAccount: true,
};
