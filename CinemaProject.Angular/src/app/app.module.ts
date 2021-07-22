import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavigationComponent } from './UserPages/MainPage/navigation/navigation.component';
import { ScheduleComponent } from './UserPages/MainPage/schedule/schedule.component';
import { MainComponent as UserMainComponent } from './UserPages/MainPage/main/main.component';
import { AuthFormComponent } from './UserPages/AuthorizationPage/auth-form/auth-form.component';
import { HeaderComponent as UserHeaderComponent } from './UserPages/MainPage/header/header.component';
import { MainComponent as AdminMainComponent } from './AdminPages/MainPage/main/main.component';
import { HeaderComponent as AdminHeaderComponent } from './AdminPages/MainPage/header/header.component';
import { TableComponent } from './AdminPages/MainPage/table/table.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    ScheduleComponent,
    UserMainComponent,
    AuthFormComponent,
    UserHeaderComponent,
    AdminMainComponent,
    AdminHeaderComponent,
    TableComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    Ng2SmartTableModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
