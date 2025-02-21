import { KeycloakService } from 'keycloak-angular';
import { environment } from './environments/environment';

export function initKeycloak(keycloak: KeycloakService) {
  return () =>
    keycloak.init({
      config: {
        url: environment.keycloakEndpoint,
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
