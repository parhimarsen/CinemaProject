import { NgModule, InjectionToken } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputCounterModule } from '@angular-material-extensions/input-counter';
import {
  NgxMatDatetimePickerModule,
  NgxMatTimepickerModule,
} from '@angular-material-components/datetime-picker';
import { NgxMatNativeDateModule } from '@angular-material-components/datetime-picker';
import { MatIconModule } from '@angular/material/icon';
import { ModalModule, BsModalService } from 'ngx-bootstrap/modal';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { MainComponent as AdminMainComponent } from './MainPage/main/main.component';
import { HeaderComponent as AdminHeaderComponent } from './MainPage/header/header.component';
import { TableComponent } from './MainPage/table/table.component';
import { CinemasService } from './services/cinemas.service';
import { FilmsService } from './services/films.service';
import { HallsService } from './services/halls.service';
import { DatePickerComponent } from './MainPage/custom/date-picker/date-picker.component';
import { Ng2InputComponent } from './MainPage/custom/input/ng2-input.component';
import { InputComponent } from './MainPage/custom/input/input.component';
import { DurationPickerComponent } from './MainPage/custom/duration-picker/duration-picker.component';
import { SelectComponent } from './MainPage/custom/select/select.component';
import { HallComponent } from './MainPage/hall/hall.component';
import { SeatComponent } from './MainPage/seat/seat.component';
import { TypeOfSeatComponent } from './MainPage/type-of-seat/type-of-seat.component';
import { SelectEditComponent } from './MainPage/custom/select-edit/select-edit.component';
import { AmenitiesService } from './services/amenities.service';
import { CurrencyInputComponent } from './MainPage/custom/currency-input/currency-input.component';
import { SessionsService } from './services/sessions.service';
import { DateTimePickerComponent } from './MainPage/custom/date-time-picker/date-time-picker.component';
import { CitiesService } from './services/cities.service';
import { EditableSelectComponent } from './MainPage/custom/editable-select/editable-select.component';
import { ModalComponent } from './MainPage/custom/modal/modal.component';
import { AdminGuard } from '../UserPages/AuthorizationPage/helpers/admin.guard';

const CINEMAS_SERVICE_TOKEN = new InjectionToken<string>('CinemasService');
const HALLS_SERVICE_TOKEN = new InjectionToken<string>('HallsService');
const FILMS_SERVICE_TOKEN = new InjectionToken<string>('FilmsService');
const SERVICES_SERVICE_TOKEN = new InjectionToken<string>('AmenitiesService');
const SESSIONS_SERVICE_TOKEN = new InjectionToken<string>('SessionsService');
const CITIES_SERVICE_TOKEN = new InjectionToken<string>('CitiesService');

const routes: Routes = [
  {
    path: 'admin/cinemas',
    component: AdminMainComponent,
    data: { requiredService: CINEMAS_SERVICE_TOKEN },
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/halls',
    component: AdminMainComponent,
    data: { requiredService: HALLS_SERVICE_TOKEN },
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/films',
    component: AdminMainComponent,
    data: { requiredService: FILMS_SERVICE_TOKEN },
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/services',
    component: AdminMainComponent,
    data: { requiredService: SERVICES_SERVICE_TOKEN },
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/sessions',
    component: AdminMainComponent,
    data: { requiredService: SESSIONS_SERVICE_TOKEN },
    canActivate: [AdminGuard],
  },
  {
    path: 'admin/cities',
    component: AdminMainComponent,
    data: { requiredService: CITIES_SERVICE_TOKEN },
    canActivate: [AdminGuard],
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
    Ng2InputComponent,
    InputComponent,
    DurationPickerComponent,
    SelectComponent,
    HallComponent,
    SeatComponent,
    TypeOfSeatComponent,
    SelectEditComponent,
    CurrencyInputComponent,
    DateTimePickerComponent,
    EditableSelectComponent,
    ModalComponent,
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
    ReactiveFormsModule,
    NgxMatDatetimePickerModule,
    NgxMatTimepickerModule,
    NgxMatNativeDateModule,
    MatIconModule,
    ModalModule,
    MatCheckboxModule,
  ],
  providers: [
    {
      provide: CINEMAS_SERVICE_TOKEN,
      useClass: CinemasService,
    },
    {
      provide: HALLS_SERVICE_TOKEN,
      useClass: HallsService,
    },
    {
      provide: FILMS_SERVICE_TOKEN,
      useClass: FilmsService,
    },
    {
      provide: SERVICES_SERVICE_TOKEN,
      useClass: AmenitiesService,
    },
    {
      provide: SESSIONS_SERVICE_TOKEN,
      useClass: SessionsService,
    },
    {
      provide: CITIES_SERVICE_TOKEN,
      useClass: CitiesService,
    },
    CurrencyPipe,
    BsModalService,
  ],
  exports: [RouterModule],
  entryComponents: [DatePickerComponent],
})
export class AdminModule {}
