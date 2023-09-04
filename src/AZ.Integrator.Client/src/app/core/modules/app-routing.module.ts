import { NgModule } from '@angular/core';
import { mapToCanActivate, RouterModule, Routes } from '@angular/router';
import { NavigationComponent } from '../ui/navigation/navigation.component';
import { AuthGuard } from '../guards/auth.guard';
import { LoginComponent } from '../ui/login/login/login.component';
import { LoginScreenGuard } from '../guards/login-screen.guard';

export const RoutePaths = {
  Auth: 'auth',
  Login: 'login',
  Test: 'test',
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
        canActivate: mapToCanActivate([AuthGuard]),
      },
    ],
  },
  {
    path: RoutePaths.Login,
    component: LoginComponent,
    canActivate: mapToCanActivate([LoginScreenGuard]),
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
