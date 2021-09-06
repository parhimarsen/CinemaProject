import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthFormComponent } from './UserPages/AuthorizationPage/auth-form/auth-form.component';
import { UserGuard } from './UserPages/AuthorizationPage/helpers/user.guard';
import { MainComponent as UserMainComponent } from './UserPages/MainPage/main/main.component';
import { AuthGuard } from './UserPages/AuthorizationPage/helpers/auth.guard';

const routes: Routes = [
  {
    path: 'user/main',
    component: UserMainComponent,
    canActivate: [UserGuard],
  },
  { path: 'auth', component: AuthFormComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/auth', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
