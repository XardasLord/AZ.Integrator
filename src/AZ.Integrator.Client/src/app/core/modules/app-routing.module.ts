import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NavigationComponent } from '../ui/navigation/navigation.component';

export const RoutePaths = {
  Auth: 'auth',
  Login: 'login',
  Orders: 'orders',
  ParcelTemplates: 'parcel-templates',
};

const routes: Routes = [
  {
    path: '',
    component: NavigationComponent,
    children: [
      {
        path: '',
        redirectTo: RoutePaths.Orders,
        pathMatch: 'full',
      },
      {
        path: RoutePaths.Orders,
        loadChildren: () => import('../../features/orders/orders.module').then(m => m.OrdersModule),
        // canActivate: mapToCanActivate([AuthGuard]),
      },
      {
        path: RoutePaths.ParcelTemplates,
        loadChildren: () =>
          import('../../features/package-templates/parcel-templates.module').then(m => m.ParcelTemplatesModule),
        // canActivate: mapToCanActivate([AuthGuard]),
      },
    ],
  },
  // {
  //   path: RoutePaths.Login,
  //   component: LoginComponent,
  //   canActivate: mapToCanActivate([LoginScreenGuard]),
  // },
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
