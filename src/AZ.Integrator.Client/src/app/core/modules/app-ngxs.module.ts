import { isDevMode, NgModule } from '@angular/core';
import { provideStates, provideStore, withNgxsDevelopmentOptions } from '@ngxs/store';
import { withNgxsLoggerPlugin } from '@ngxs/logger-plugin';
import { withNgxsRouterPlugin } from '@ngxs/router-plugin';
import { NgxsReduxDevtoolsPluginModule, withNgxsReduxDevtoolsPlugin } from '@ngxs/devtools-plugin';
import { withNgxsFormPlugin } from '@ngxs/form-plugin';
import { AuthState } from '../../shared/states/auth.state';
import { TenantState } from '../../shared/states/tenant.state';

@NgModule({
  imports: [],
  exports: [NgxsReduxDevtoolsPluginModule],
  providers: [
    provideStore(
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
    ),
    provideStates([]),
  ],
})
export class AppNgxsModule {}
