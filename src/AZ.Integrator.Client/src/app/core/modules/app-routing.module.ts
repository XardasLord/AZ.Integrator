import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NavigationComponent } from '../ui/navigation/navigation.component';

export const RoutePaths = {
  Auth: 'auth',
  Login: 'login',
  AllegroOrders: 'allegro-orders',
  ParcelTemplates: 'parcel-templates',
};

const routes: Routes = [
  {
    path: '',
    component: NavigationComponent,
    children: [
      {
        path: '',
        redirectTo: RoutePaths.AllegroOrders,
        pathMatch: 'full',
      },
      {
        path: RoutePaths.AllegroOrders,
        loadChildren: () =>
          import('../../features/allegro-orders/allegro-orders.module').then(m => m.AllegroOrdersModule),
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
