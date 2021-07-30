import { NgModule, InjectionToken } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputCounterModule } from '@angular-material-extensions/input-counter';

import { MainComponent as AdminMainComponent } from './MainPage/main/main.component';
import { HeaderComponent as AdminHeaderComponent } from './MainPage/header/header.component';
import { TableComponent } from './MainPage/table/table.component';
import { CinemasService } from './services/cinemas.service';
import { FilmsService } from './services/films.service';
import { DatePickerComponent } from './MainPage/custom/date-picker/date-picker.component';
import { InputComponent } from './MainPage/custom/input/input.component';
import { DurationPickerComponent } from './MainPage/custom/duration-picker/duration-picker.component';
import { SelectComponent } from './MainPage/custom/select/select.component';
import { HallComponent } from './MainPage/hall/hall.component';
import { SeatComponent } from './MainPage/seat/seat.component';
import { TypeOfSeatComponent } from './MainPage/type-of-seat/type-of-seat.component';

const CINEMAS_SERVICE_TOKEN = new InjectionToken<string>('CinemasService');
const FILMS_SERVICE_TOKEN = new InjectionToken<string>('FilmsService');

const routes: Routes = [
  { path: 'admin', redirectTo: 'admin/cinemas', pathMatch: 'full' },
  {
    path: 'admin/cinemas',
    component: AdminMainComponent,
    data: { requiredService: CINEMAS_SERVICE_TOKEN },
  },
  {
    path: 'admin/films',
    component: AdminMainComponent,
    data: { requiredService: FILMS_SERVICE_TOKEN },
  },
  {
    path: 'admin/cinemas/:cinemaId/halls/:hallId',
    component: HallComponent,
  },
];

@NgModule({
  declarations: [
    AdminMainComponent,
    AdminHeaderComponent,
    TableComponent,
    DatePickerComponent,
    InputComponent,
    DurationPickerComponent,
    SelectComponent,
    HallComponent,
    SeatComponent,
    TypeOfSeatComponent,
  ],
  imports: [
    RouterModule.forRoot(routes),
    CommonModule,
    FormsModule,
    Ng2SmartTableModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatInputCounterModule,
  ],
  providers: [
    {
      provide: CINEMAS_SERVICE_TOKEN,
      useClass: CinemasService,
      //multi: true,
    },
    {
      provide: FILMS_SERVICE_TOKEN,
      useClass: FilmsService,
    },
  ],
  exports: [RouterModule],
  entryComponents: [DatePickerComponent],
})
export class AdminModule {}
