import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AdminModule } from './AdminPages/admin.module';

import { AppComponent } from './app.component';
import { NavigationComponent } from './UserPages/MainPage/navigation/navigation.component';
import { ScheduleComponent } from './UserPages/MainPage/schedule/schedule.component';
import { MainComponent as UserMainComponent } from './UserPages/MainPage/main/main.component';
import { AuthFormComponent } from './UserPages/AuthorizationPage/auth-form/auth-form.component';
import { HeaderComponent as UserHeaderComponent } from './UserPages/MainPage/header/header.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    ScheduleComponent,
    UserMainComponent,
    AuthFormComponent,
    UserHeaderComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AdminModule,
    BrowserAnimationsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
