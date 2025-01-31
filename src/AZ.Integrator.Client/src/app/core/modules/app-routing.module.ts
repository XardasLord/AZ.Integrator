import { NgModule } from '@angular/core';
import { mapToCanActivate, RouterModule, Routes } from '@angular/router';
import { provideStates } from '@ngxs/store';
import { NavigationComponent } from '../ui/navigation/navigation.component';
import { LoginCompletedComponent } from '../ui/login-completed/login-completed.component';
import { AuthGuard } from '../guards/auth.guard';
import { RouteAuthVo } from '../../shared/auth/models/route-auth.vo';
import { AuthRoles } from '../../shared/auth/models/auth.roles';
import { NotAuthorizedComponent } from '../ui/not-authorized/not-authorized.component';
import { HomeComponent } from '../ui/home/home.component';
import { StocksState } from '../../features/stocks/states/stocks.state';
import { StocksService } from '../../features/stocks/services/stocks.service';
import { BarcodeScannerState } from '../../features/stocks/states/barcode-scanner.state';

export const RoutePaths = {
  Auth: 'auth',
  NotAuthorized: 'not-authorized',
  Login: 'login',
  LoginCompleted: 'login-completed',
  Home: 'home',
  Orders: 'orders',
  ParcelTemplates: 'parcel-templates',
  Stocks: 'stocks',
  BarcodeScanner: 'barcode-scanner',
};

const routes: Routes = [
  {
    path: '',
    component: NavigationComponent,
    children: [
      {
        path: '',
        redirectTo: RoutePaths.Home,
        pathMatch: 'full',
      },
      {
        path: RoutePaths.Home,
        component: HomeComponent,
      },
      {
        path: RoutePaths.LoginCompleted,
        component: LoginCompletedComponent,
      },
      {
        path: RoutePaths.NotAuthorized,
        component: NotAuthorizedComponent,
      },
      {
        path: RoutePaths.Orders,
        loadChildren: () => import('../../features/orders/orders.module').then(m => m.OrdersModule),
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin],
        }),
      },
      {
        path: RoutePaths.ParcelTemplates,
        loadChildren: () =>
          import('../../features/package-templates/parcel-templates.module').then(m => m.ParcelTemplatesModule),
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin],
        }),
      },
      {
        path: RoutePaths.Stocks,
        loadComponent: () => import('../../features/stocks/pages/stocks/stocks.component').then(c => c.StocksComponent),
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin],
        }),
        providers: [provideStates([StocksState]), StocksService],
      },
      {
        path: RoutePaths.BarcodeScanner,
        loadComponent: () =>
          import('../../features/stocks/pages/barcode-scanner-page/barcode-scanner-page.component').then(
            c => c.BarcodeScannerPageComponent
          ),
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.ScannerIn, AuthRoles.ScannerOut],
        }),
        providers: [provideStates([BarcodeScannerState]), StocksService],
      },
    ],
  },
  {
    path: '**',
    redirectTo: '',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
