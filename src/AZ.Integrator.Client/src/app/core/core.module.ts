import { ErrorHandler, NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { AppRoutingModule } from './modules/app-routing.module';
import { NavigationComponent } from './ui/navigation/navigation.component';
import { ToolbarComponent } from './ui/toolbar/toolbar.component';
import { AppNgxsModule } from './modules/app-ngxs.module';
import { GlobalErrorHandler } from './interceptor/error-handler.interceptor';
import { LoginComponent } from './ui/login/login/login.component';
import { AuthInterceptor } from './interceptor/auth.interceptor';
import { AuthGuard } from './guards/auth.guard';
import { NoCacheInterceptor } from './interceptor/no-cache.interceptor';
import { AppGraphQLModule } from './modules/app-graphql.module';
import { UnauthorizedInterceptor } from './interceptor/unauthorized.interceptor';
import { LoadingInterceptor } from './interceptor/loading.interceptor';

@NgModule({
  declarations: [NavigationComponent, ToolbarComponent, LoginComponent],
  imports: [SharedModule, AppRoutingModule, AppNgxsModule, AppGraphQLModule],
  providers: [
    AuthGuard,
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: NoCacheInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: UnauthorizedInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
  ],
})
export class CoreModule {}
