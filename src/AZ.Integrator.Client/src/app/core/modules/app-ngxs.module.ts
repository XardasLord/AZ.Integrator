import { isDevMode, NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { AuthState } from '../../shared/states/auth.state';

@NgModule({
  imports: [
    NgxsModule.forRoot([AuthState], {
      developmentMode: isDevMode(),
      selectorOptions: {
        suppressErrors: false,
        injectContainerState: false,
      },
    }),
    NgxsModule.forFeature([]),
    NgxsLoggerPluginModule.forRoot({
      collapsed: true,
      disabled: !isDevMode(),
    }),
    NgxsReduxDevtoolsPluginModule.forRoot({
      name: 'Integrator',
      disabled: !isDevMode(),
    }),
    NgxsRouterPluginModule.forRoot(),
    NgxsFormPluginModule.forRoot(),
  ],
  exports: [NgxsReduxDevtoolsPluginModule],
})
export class AppNgxsModule {}
