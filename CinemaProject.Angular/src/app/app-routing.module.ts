import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthFormComponent } from './UserPages/AuthorizationPage/auth-form/auth-form.component';
import { MainComponent as UserMainComponent } from './UserPages/MainPage/main/main.component';
import { MainComponent as AdminMainComponent } from './AdminPages/MainPage/main/main.component';

const routes: Routes = [
  { path: '', redirectTo: 'user/main', pathMatch: 'full' },
  { path: 'user/main', component: UserMainComponent },
  { path: 'auth', component: AuthFormComponent },
  { path: 'admin/main', component: AdminMainComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
