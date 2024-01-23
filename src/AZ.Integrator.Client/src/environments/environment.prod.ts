import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}`,
  apiEndpoint: 'http://desktop-vvm1ebh:6010/api',
  graphqlEndpoint: 'http://desktop-vvm1ebh:6010/api/graphql',
  defaultPageSize: 10,
  allegroLoginEndpointForAzTeamTenant: 'http://desktop-vvm1ebh:6010/auth/login-allegro?tenantId=az-team',
  allegroLoginEndpointForMebleplTenant: 'http://desktop-vvm1ebh:6010/auth/login-allegro?tenantId=meblepl',
  allegroLoginEndpointForMyTestTenant: 'http://desktop-vvm1ebh:6010/auth/login-allegro?tenantId=my-test',

  showMyTestAllegroAccount: false,
};
