import { bootstrapApplication, BrowserModule, enableDebugTools } from '@angular/platform-browser';
import { APP_INITIALIZER, ApplicationRef, importProvidersFrom, inject, isDevMode } from '@angular/core';

import { initKeycloak } from './init.keycloak';
import { KeycloakAngularModule, KeycloakService } from 'keycloak-angular';
import { AppRoutingModule } from './app/core/modules/app-routing.module';
import { provideAnimations } from '@angular/platform-browser/animations';
import { SharedModule } from './app/shared/shared.module';
import { CoreModule } from './app/core/core.module';
import { AppComponent } from './app/app.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideStates, provideStore, withNgxsDevelopmentOptions } from '@ngxs/store';
import { AuthState } from './app/shared/states/auth.state';
import { TenantState } from './app/shared/states/tenant.state';
import { withNgxsLoggerPlugin } from '@ngxs/logger-plugin';
import { withNgxsReduxDevtoolsPlugin } from '@ngxs/devtools-plugin';
import { withNgxsRouterPlugin } from '@ngxs/router-plugin';
import { withNgxsFormPlugin } from '@ngxs/form-plugin';
import { DictionaryState } from './app/shared/states/dictionary.state';
import { provideApollo } from 'apollo-angular';
import { HttpLink } from 'apollo-angular/http';
import { environment } from './environments/environment';
import { InMemoryCache } from '@apollo/client/core';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(BrowserModule, AppRoutingModule, SharedModule, CoreModule, KeycloakAngularModule),
    {
      provide: APP_INITIALIZER,
      useFactory: initKeycloak,
      multi: true,
      deps: [KeycloakService],
    },
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()),
    provideNgxsStore(),
    provideStates([DictionaryState]),
    provideGraphQL(),
  ],
})
  .then(module => enableDebugTools(module.injector.get(ApplicationRef).components[0]))
  .catch(err => console.error(err));

function provideNgxsStore() {
  return provideStore(
    [AuthState, TenantState],
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
