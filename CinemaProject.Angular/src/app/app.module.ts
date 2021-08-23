import { APP_INITIALIZER, NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AdminModule } from './AdminPages/admin.module';

import { AppComponent } from './app.component';
import { NavigationComponent } from './UserPages/MainPage/navigation/navigation.component';
import { ScheduleComponent } from './UserPages/MainPage/schedule/schedule.component';
import { MainComponent as UserMainComponent } from './UserPages/MainPage/main/main.component';
import { AuthFormComponent } from './UserPages/AuthorizationPage/auth-form/auth-form.component';
import { HeaderComponent as UserHeaderComponent } from './UserPages/MainPage/header/header.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { appInitializer } from './UserPages/AuthorizationPage/helpers/app.initializer';
import { AuthService } from './UserPages/AuthorizationPage/services/auth.service';
import { ErrorInterceptor } from './UserPages/AuthorizationPage/helpers/error.interceptor';
import { JwtInterceptor } from './UserPages/AuthorizationPage/helpers/jwt.interceptor';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { InputComponent } from './UserPages/AuthorizationPage/custom/input/input.component';
import { FormComponent } from './UserPages/AuthorizationPage/custom/form/form.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    ScheduleComponent,
    UserMainComponent,
    AuthFormComponent,
    UserHeaderComponent,
    InputComponent,
    FormComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AdminModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializer,
      multi: true,
      deps: [AuthService],
    },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
