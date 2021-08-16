import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  ViewChild,
} from '@angular/core';
import { DefaultEditor } from 'ng2-smart-table';

import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { CinemasService } from 'src/app/AdminPages/services/cinemas.service';
import { CitiesService } from 'src/app/AdminPages/services/cities.service';
import { FilmsService } from 'src/app/AdminPages/services/films.service';
import { Cinema, CinemaView } from 'src/app/AdminPages/Models/cinema';
import { HallView } from 'src/app/AdminPages/Models/hall';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-select-edit',
  templateUrl: './select-edit.component.html',
  styleUrls: ['./select-edit.component.css'],
})
export class SelectEditComponent
  extends DefaultEditor
  implements AfterViewInit
{
  //Get value from hidden div
  @ViewChild('htmlValue') htmlValue!: ElementRef;
  //Values in drop-down list
  value!: any[];
  //Needed for adding Halls in drop-down list after Cinema was selected in another drop-down list
  static onSelectCinema: EventEmitter<any>;
  //Validation
  formGroup: FormGroup;
  selectedValue!: string | undefined;
  url: string[];
  savedCinemaName!: string;
  savedHallName: string | undefined;
  isFlag: boolean = true;

  constructor(
    private cdr: ChangeDetectorRef,
    private cinemasService: CinemasService,
    private filmsService: FilmsService,
    private citiesService: CitiesService,
    private router: Router
  ) {
    super();
    this.url = this.router.url.split('/');
    this.formGroup = new FormGroup({
      value: new FormControl(
        { value: '', disabled: false },
        Validators.required
      ),
    });
    this.formGroup.markAllAsTouched();
  }

  ngAfterViewInit(): void {
    if (this.cell.newValue !== '') {
      this.selectedValue = this.getValue();
      this.cdr.detectChanges();
    }

    //Cinemas drop-down list has situation when need to load Halls after Cinema was selected
    if (this.cell.getTitle() === 'Film') {
      this.filmsService.getAll().subscribe((films) => {
        this.value = films;
      });
    } else if (this.cell.getTitle() === 'Cinema') {
      if (this.url[this.url.length - 1] === 'sessions')
        SelectEditComponent.onSelectCinema = new EventEmitter<any>();
      this.setCinemasWithCities();
    } else if (this.cell.getTitle() === 'Hall') {
      if (SelectEditComponent.onSelectCinema) {
        SelectEditComponent.onSelectCinema.subscribe((cinema) => {
          if (this.isFlag) {
            this.savedCinemaName = cinema.name;
            this.savedHallName = this.selectedValue;
            this.isFlag = false;
          }

          if (this.savedCinemaName && this.savedCinemaName === cinema.name) {
            this.selectedValue = this.savedHallName;
          }

          this.value = cinema.halls;
          if (cinema.halls.length === 0) this.selectedValue = undefined;
          this.cell.newValue = this.selectedValue;
          this.cdr.detectChanges();
        });
      }
    } else if (this.cell.getTitle() === 'City') {
      this.citiesService.getAll().subscribe((citites) => {
        this.value = citites;
      });
    }
    this.cdr.detectChanges();
  }

  private setCinemasWithCities() {
    forkJoin({
      cinemas: this.cinemasService.getAll(),
      cities: this.citiesService.getAll(),
    }).subscribe((response) => {
      this.value = response.cinemas.map((cinema) => {
        let cityName = response.cities.find((city) => {
          return city.id === cinema.cityId;
        })?.name;
        return new CinemaView(
          cinema.id,
          cinema.name + ' / ' + cityName,
          cinema
            .halls!.sort((a, b) => parseInt(a.name) - parseInt(b.name))
            .map((hall) => {
              return new HallView(hall.id, hall.name, cinema.id, cinema.name);
            }),
          cinema.typesOfSeat!,
          cityName
        );
      });

      //Emulates the situation when Cinema already selected in Edit Mode
      if (
        this.cell.getTitle() === 'Cinema' &&
        this.url[this.url.length - 1] === 'sessions'
      ) {
        this.value.forEach((cinema) => {
          if (cinema.name === this.selectedValue) {
            SelectEditComponent.onSelectCinema.emit(cinema);
            this.cell.newValue = cinema;
          }
        });
      }
    });
  }

  updateItem(event: any) {
    if (this.cell.getTitle() === 'Cinema') {
      this.value.forEach((cinema) => {
        if (cinema.name === this.selectedValue) {
          if (this.url[this.url.length - 1] === 'sessions')
            SelectEditComponent.onSelectCinema.emit(cinema);
          this.cell.newValue = cinema;
        }
      });
    } else {
      this.cell.newValue = event.value;
    }
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
