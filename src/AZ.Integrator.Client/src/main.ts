import { bootstrapApplication, BrowserModule, enableDebugTools } from '@angular/platform-browser';
import { ApplicationRef, importProvidersFrom, inject, isDevMode } from '@angular/core';
import {
  INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
  includeBearerTokenInterceptor,
  provideKeycloak,
} from 'keycloak-angular';
import { AppRoutingModule } from './app/core/modules/app-routing.module';
import { SharedModule } from './app/shared/shared.module';
import { CoreModule } from './app/core/core.module';
import { AppComponent } from './app/app.component';
import { provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { provideStates, provideStore, withNgxsDevelopmentOptions } from '@ngxs/store';
import { AuthState } from './app/shared/states/auth.state';
import { SourceSystemState } from './app/shared/states/source-system.state';
import { withNgxsLoggerPlugin } from '@ngxs/logger-plugin';
import { withNgxsReduxDevtoolsPlugin } from '@ngxs/devtools-plugin';
import { withNgxsRouterPlugin } from '@ngxs/router-plugin';
import { withNgxsFormPlugin } from '@ngxs/form-plugin';
import { DictionaryState } from './app/shared/states/dictionary.state';
import { provideApollo } from 'apollo-angular';
import { HttpLink } from 'apollo-angular/http';
import { environment } from './environments/environment';
import { InMemoryCache } from '@apollo/client/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { FeatureFlagsState } from './app/shared/feature-flags/store/feature-flags.state.ts';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(BrowserModule, AppRoutingModule, SharedModule, CoreModule),
    provideHttpClient(withInterceptors([includeBearerTokenInterceptor]), withInterceptorsFromDi()),

    {
      provide: INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
      useValue: [
        { urlPattern: new RegExp(`^${environment.apiEndpoint}`) },
        { urlPattern: new RegExp(`^${environment.graphqlEndpoint}`) },
      ],
    },

    provideKeycloak({
      config: {
        url: environment.keycloakEndpoint,
        realm: 'az-integrator',
        clientId: 'az-integrator-client',
      },
      initOptions: {
        onLoad: 'check-sso',
        silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
        checkLoginIframe: false,
        pkceMethod: 'S256',
      },
      // features: [withAutoRefreshToken()],
    }),

    provideAnimations(),
    provideNgxsStore(),
    provideStates([DictionaryState, FeatureFlagsState]),
    provideGraphQL(),
  ],
})
  .then(module => enableDebugTools(module.injector.get(ApplicationRef).components[0]))
  .catch(err => console.error(err));

function provideNgxsStore() {
  return provideStore(
    [AuthState, SourceSystemState],
    withNgxsLoggerPlugin({
      collapsed: true,
      disabled: !isDevMode(),
    }),
    withNgxsDevelopmentOptions({ warnOnUnhandledActions: true }),
    withNgxsReduxDevtoolsPlugin({
      name: 'AZ-Integrator',
      disabled: !isDevMode(),
    }),
    withNgxsRouterPlugin({}),
    withNgxsFormPlugin()
  );
}

function provideGraphQL() {
  return provideApollo(() => {
    const httpLink = inject(HttpLink);

    return {
      link: httpLink.create({ uri: environment.graphqlEndpoint }),
      cache: new InMemoryCache(),
      defaultOptions: {
        watchQuery: {
          fetchPolicy: 'network-only',
        },
      },
    };
  });
}

export const GraphQLIntegratorClientName = 'default';
