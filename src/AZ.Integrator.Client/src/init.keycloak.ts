import { KeycloakService } from 'keycloak-angular';

export function initKeycloak(keycloak: KeycloakService) {
  return () =>
    keycloak.init({
      config: {
        url: 'http://localhost:9080',
        realm: 'az-integrator',
        clientId: 'az-integrator-client',
      },
      initOptions: {
        onLoad: 'check-sso',
        checkLoginIframe: false,
        enableLogging: true,
      },
      enableBearerInterceptor: true,
      bearerPrefix: 'Bearer',
    });
}
