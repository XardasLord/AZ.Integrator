import { APP_VERSION } from '../app/version';

export const environment = {
  version: `${APP_VERSION}-dev`,
  apiEndpoint: 'http://localhost:6010/api',
  graphqlEndpoint: 'http://localhost:6010/api/graphql',
  defaultPageSize: 10,
  allegroLoginEndpoint: 'http://localhost:6010/auth/login-allegro',
  allegroApiUrl: 'https://allegro.pl/auth/oauth/authorize',
  allegroApiRedirectUriCallback: 'http://localhost:6010/auth/allegro-auth-callback',
  allegroApiClientId: '869fe88a9cca4602b47e363716e9e7a7',
  allegroApiClientSecret: 'sQq2uYXrPemwKr2s1msoow8wWqvz84lFiMqVbnj1KrD6Z74n1ciNjbotK7AQGedi',
};
