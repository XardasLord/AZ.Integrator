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
import { StockLogsState } from '../../features/stocks/states/stock-logs.state';
import { StockGroupsService } from '../../features/stocks/services/stock-groups.service';
import { FormatsState } from '../../features/furniture-formats/formats/states/formats.state';
import { FurnitureFormatsService } from '../../features/furniture-formats/formats/services/furniture-formats.service';
import { SuppliersState } from '../../features/furniture-formats/suppliers/states/suppliers.state';
import { SuppliersService } from '../../features/furniture-formats/suppliers/services/suppliers.service';
import { OrdersState } from '../../features/furniture-formats/orders/states/orders.state';
import { OrdersService } from '../../features/furniture-formats/orders/services/orders.service';
import { IntegrationsState } from '../../features/integrations/states/integrations.state';

export const RoutePaths = {
  Auth: 'auth',
  NotAuthorized: 'not-authorized',
  Login: 'login',
  LoginCompleted: 'login-completed',
  Home: 'home',
};

export const MarketplaceRoutePath = {
  Orders: 'marketplace/orders',
  ParcelTemplates: 'marketplace/parcel-templates',
};

export const StocksRoutePath = {
  Stocks: 'warehouse/stocks',
  Statistics: 'warehouse/stocks-statistics',
  BarcodeScanner: 'warehouse/barcode-scanner',
};

export const FurnitureFormatsRoutePath = {
  Formats: 'furniture-formats/bom',
  Suppliers: 'furniture-formats/suppliers',
  Orders: 'furniture-formats/orders',
  OrdersCreate: 'furniture-formats/orders/create',
};

export const IntegrationsRoutePath = {
  Integrations: 'integrations',
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
        path: MarketplaceRoutePath.Orders,
        loadChildren: () => import('../../features/orders/orders.module').then(m => m.OrdersModule),
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin, AuthRoles.Operator],
        }),
        providers: [provideStates([IntegrationsState])],
      },
      {
        path: MarketplaceRoutePath.ParcelTemplates,
        loadChildren: () =>
          import('../../features/package-templates/parcel-templates.module').then(m => m.ParcelTemplatesModule),
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin, AuthRoles.Operator],
        }),
      },

      {
        path: StocksRoutePath.Stocks,
        loadComponent: () => import('../../features/stocks/pages/stocks/stocks.component').then(c => c.StocksComponent),
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin, AuthRoles.Operator],
        }),
        providers: [provideStates([StocksState]), StocksService, StockGroupsService],
      },
      {
        path: StocksRoutePath.Statistics,
        loadComponent: () =>
          import('../../features/stocks/pages/stocks-statistics/stocks-statistics.component').then(
            c => c.StocksStatisticsComponent
          ),
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin],
        }),
        providers: [provideStates([StockLogsState]), StocksService],
      },
      {
        path: StocksRoutePath.BarcodeScanner,
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

      {
        path: FurnitureFormatsRoutePath.Formats,
        loadComponent: () =>
          import('../../features/furniture-formats/formats/pages/formats/formats.component').then(
            c => c.FormatsComponent
          ),
        providers: [provideStates([FormatsState]), FurnitureFormatsService],
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin, AuthRoles.Operator],
        }),
      },
      {
        path: FurnitureFormatsRoutePath.Suppliers,
        loadComponent: () =>
          import('../../features/furniture-formats/suppliers/pages/suppliers/suppliers.component').then(
            c => c.SuppliersComponent
          ),
        providers: [provideStates([SuppliersState]), SuppliersService],
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin, AuthRoles.Operator],
        }),
      },
      {
        path: FurnitureFormatsRoutePath.Orders,
        loadComponent: () =>
          import('../../features/furniture-formats/orders/pages/orders/orders.component').then(c => c.OrdersComponent),
        providers: [
          provideStates([OrdersState, SuppliersState, FormatsState]),
          OrdersService,
          SuppliersService,
          FurnitureFormatsService,
        ],
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin, AuthRoles.Operator],
        }),
      },
      {
        path: `${FurnitureFormatsRoutePath.OrdersCreate}`,
        loadComponent: () =>
          import('../../features/furniture-formats/orders/pages/create-order/create-order.component').then(
            c => c.CreateOrderComponent
          ),
        providers: [
          provideStates([OrdersState, SuppliersState, FormatsState]),
          OrdersService,
          SuppliersService,
          FurnitureFormatsService,
        ],
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin, AuthRoles.Operator],
        }),
      },
      {
        path: IntegrationsRoutePath.Integrations,
        loadComponent: () =>
          import('../../features/integrations/pages/integrations/integrations.component').then(
            c => c.IntegrationsComponent
          ),
        canActivate: mapToCanActivate([AuthGuard]),
        data: new RouteAuthVo({
          allowRoles: [AuthRoles.Admin],
        }),
        providers: [provideStates([IntegrationsState])],
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
