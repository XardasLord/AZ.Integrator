import { NgModule } from '@angular/core';
import { mapToCanActivate, RouterModule, Routes } from '@angular/router';
import { NavigationComponent } from '../ui/navigation/navigation.component';
import { LoginCompletedComponent } from '../ui/login-completed/login-completed.component';
import { AuthGuard } from '../guards/auth.guard';
import { RouteAuthVo } from '../../shared/auth/models/route-auth.vo';
import { AuthRoles } from '../../shared/auth/models/auth.roles';
import { NotAuthorizedComponent } from '../ui/not-authorized/not-authorized.component';
import { HomeComponent } from '../ui/home/home.component';

export const RoutePaths = {
  Auth: 'auth',
  NotAuthorized: 'not-authorized',
  Login: 'login',
  LoginCompleted: 'login-completed',
  Home: 'home',
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
