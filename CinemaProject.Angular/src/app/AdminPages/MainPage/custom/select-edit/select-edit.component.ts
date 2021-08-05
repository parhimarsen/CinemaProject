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
  selectedValue!: string;
  url: string[];

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

      //Emulates the situation when Cinema already selected in Edit Mode
      if (
        this.cell.getTitle() === 'Cinema' &&
        this.url[this.url.length - 1] !== 'halls'
      ) {
        SelectEditComponent.onSelectCinema = new EventEmitter<any>();
        this.cinemasService.getAll().subscribe((cinemas) => {
          let selectedCinema = cinemas.find(
            (cinema) => cinema.name === this.selectedValue
          );
          SelectEditComponent.onSelectCinema.emit(selectedCinema?.halls);
        });
      }
    }

    //Cinemas drop-down list has situation when need to load Halls after Cinema was selected
    if (this.cell.getTitle() === 'Film') {
      this.filmsService.getAll().subscribe((films) => {
        this.value = films;
      });
    } else if (this.cell.getTitle() === 'Cinema') {
      if (this.url[this.url.length - 1] === 'halls') {
        this.cinemasService.getAll().subscribe((cinemas) => {
          this.value = cinemas;
        });
      } else {
        SelectEditComponent.onSelectCinema = new EventEmitter<any>();
        this.cinemasService.getAll().subscribe((cinemas) => {
          this.value = cinemas;
        });
      }
    } else if (this.cell.getTitle() === 'Hall') {
      if (SelectEditComponent.onSelectCinema) {
        SelectEditComponent.onSelectCinema.subscribe((value) => {
          this.value = value;
        });
      } else {
      }
    } else if (this.cell.getTitle() === 'City') {
      this.citiesService.getAll().subscribe((citites) => {
        this.value = citites;
      });
    }

    this.cdr.detectChanges();
  }

  updateItem(event: any) {
    this.cell.newValue = event.value;
    if (
      this.cell.getTitle() === 'Cinema' &&
      this.url[this.url.length - 1] !== 'halls'
    ) {
      this.cinemasService.getAll().subscribe((cinemas) => {
        let selectedCinema = cinemas.find(
          (cinema) => cinema.name === event.value
        );
        SelectEditComponent.onSelectCinema.emit(selectedCinema?.halls);
      });
    }
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
