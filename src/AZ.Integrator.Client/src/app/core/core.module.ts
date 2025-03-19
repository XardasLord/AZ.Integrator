import { ErrorHandler, NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { AppRoutingModule } from './modules/app-routing.module';
import { NavigationComponent } from './ui/navigation/navigation.component';
import { ToolbarComponent } from './ui/toolbar/toolbar.component';
import { GlobalErrorHandler } from './interceptor/error-handler.interceptor';
import { LoginComponent } from './ui/login/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { NoCacheInterceptor } from './interceptor/no-cache.interceptor';
import { UnauthorizedInterceptor } from './interceptor/unauthorized.interceptor';
import { LoadingInterceptor } from './interceptor/loading.interceptor';
import { LoginCompletedComponent } from './ui/login-completed/login-completed.component';
import { TenantHeadersInterceptor } from './interceptor/tenant-headers.interceptor';
import { NotAuthorizedComponent } from './ui/not-authorized/not-authorized.component';
import { HomeComponent } from './ui/home/home.component';
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';

export const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'DD.MM.YYYY',
  },
  display: {
    dateInput: 'DD.MM.YYYY',
    monthYearLabel: 'MMM YYYY',
  },
};

@NgModule({
  imports: [
    SharedModule,
    AppRoutingModule,
    NavigationComponent,
    ToolbarComponent,
    HomeComponent,
    LoginComponent,
    LoginCompletedComponent,
    NotAuthorizedComponent,
  ],
  providers: [
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
    { provide: MAT_DATE_LOCALE, useValue: 'pl-PL' },
    AuthGuard,
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TenantHeadersInterceptor,
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
