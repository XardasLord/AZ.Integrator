import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NavigationComponent } from '../ui/navigation/navigation.component';

export const RoutePaths = {
  Auth: 'auth',
  Login: 'login',
  Test: 'test',
  AllegroOrders: 'allegro-orders',
};

const routes: Routes = [
  {
    path: '',
    component: NavigationComponent,
    children: [
      {
        path: '',
        redirectTo: RoutePaths.Test,
        pathMatch: 'full',
      },
      {
        path: RoutePaths.Test,
        loadChildren: () => import('../../features/test/test.module').then(m => m.TestModule),
        // canActivate: mapToCanActivate([AuthGuard]),
      },
      {
        path: RoutePaths.AllegroOrders,
        loadChildren: () =>
          import('../../features/allegro-orders/allegro-orders.module').then(m => m.AllegroOrdersModule),
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
