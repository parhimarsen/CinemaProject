import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DatePickerComponent } from '../MainPage/custom/date-picker/date-picker.component';
import { Ng2InputComponent } from '../MainPage/custom/input/ng2-input.component';
import { DurationPickerComponent } from '../MainPage/custom/duration-picker/duration-picker.component';

import { Film, FilmView } from '../Models/film';
import { InputValidator } from '../MainPage/custom/validators/input-validator';

@Injectable({
  providedIn: 'root',
})
export class FilmsService {
  url = 'https://localhost:44356/api/Films';

  isComplited = new BehaviorSubject<boolean>(false);

  films: Film[];
  filmsView: FilmView[];
  source: LocalDataSource;
  settings: any;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {
    this.films = [];
    this.filmsView = [];
    this.source = new LocalDataSource();
  }

  private handleError<Film>(operation = 'operation', result?: Film) {
    return (error: any): Observable<Film> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as Film);
    };
  }

  getAll(): Observable<Film[]> {
    return this.http
      .get<Film[]>(this.url)
      .pipe(catchError(this.handleError<Film[]>('getFilms', [])));
  }

  private postRequest(film: Film): Observable<Film> {
    return this.http
      .post<Film>(this.url, film, this.httpOptions)
      .pipe(catchError(this.handleError<Film>('addFilm')));
  }

  private deleteRequest(id: string): Observable<Film> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<Film>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Film>('deleteFilm')));
  }

  private putRequest(film: Film): Observable<any> {
    console.log(film);
    return this.http
      .put(this.url, film, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateFilm')));
  }

  private formatDate(date: Date): string {
    let newDate = date.toString().slice(0, 10).split('-').reverse().join('-');
    return newDate;
  }

  private convertFilmToView(films: Film[]): FilmView[] {
    return films
      .map((film) => {
        return new FilmView(
          film.id,
          film.name,
          film.country,
          this.formatDate(film.releaseDate),
          film.duration,
          film.director
        );
      })
      .sort((a, b) => {
        if (a.name > b.name) {
          return 1;
        }
        if (a.name < b.name) {
          return -1;
        }
        return 0;
      });
  }

  refreshData() {
    this.getAll().subscribe((films) => {
      this.films = films;
      this.filmsView = this.convertFilmToView(this.films);
      this.source = new LocalDataSource(this.filmsView);
      this.isComplited.next(true);
    });
    this.settings = {
      add: {
        confirmCreate: true,
      },
      delete: {
        confirmDelete: true,
      },
      edit: {
        confirmSave: true,
      },
      columns: {
        name: {
          title: 'Name',
          filter: false,
          editor: {
            type: 'custom',
            component: Ng2InputComponent,
          },
        },
        country: {
          title: 'Country',
          filter: false,
          editor: {
            type: 'custom',
            component: Ng2InputComponent,
          },
        },
        releaseDate: {
          title: 'Release Date',
          filter: false,
          editor: {
            type: 'custom',
            component: DatePickerComponent,
          },
        },
        duration: {
          title: 'Duration',
          filter: false,
          editor: {
            type: 'custom',
            component: DurationPickerComponent,
          },
        },
        director: {
          title: 'Director',
          filter: false,
          editor: {
            type: 'custom',
            component: Ng2InputComponent,
          },
        },
      },
    };
  }

  onSearch(query: string) {
    if (query === '') {
      this.source.setFilter([]);
    } else {
      this.source.setFilter(
        [
          {
            field: 'name',
            search: query,
          },
          {
            field: 'country',
            search: query,
          },
          {
            field: 'releaseDate',
            search: query,
          },
          {
            field: 'duration',
            search: query,
          },
          {
            field: 'director',
            search: query,
          },
        ],
        false
      );
    }
  }

  add(event: any): void {
    let newFilm = event.newData;
    let isValid = true;

    Ng2InputComponent.onAdd.emit();
    DatePickerComponent.onAdd.emit();
    DurationPickerComponent.onAdd.emit();

    for (let field in InputValidator.isNumbersValid) {
      if (!InputValidator.isNumbersValid[field]) return;
    }
    for (let field in InputValidator.isSpacesValid) {
      if (!InputValidator.isSpacesValid[field]) return;
    }

    for (let key in newFilm) {
      if (newFilm[key] === '') {
        isValid = false;
        break;
      }

      newFilm[key] = newFilm[key].trim();
    }

    if (!isValid) return;

    //Film without Id
    newFilm = {
      name: newFilm.name,
      country: newFilm.country,
      releaseDate: newFilm.releaseDate,
      duration: newFilm.duration,
      director: newFilm.director,
    };

    this.postRequest(newFilm as Film).subscribe(() => {
      this.refreshData();
    });
  }

  delete(event: any): void {
    let film = event.data;

    this.deleteRequest(film.id).subscribe(() => {
      this.refreshData();
    });
  }

  edit(event: any): void {
    let newFilm = event.newData;
    let isValid = true;

    Ng2InputComponent.onAdd.emit();
    DatePickerComponent.onAdd.emit();
    DurationPickerComponent.onAdd.emit();

    for (let field in InputValidator.isNumbersValid) {
      if (!InputValidator.isNumbersValid[field]) return;
    }
    for (let field in InputValidator.isSpacesValid) {
      if (!InputValidator.isSpacesValid[field]) return;
    }

    for (let key in newFilm) {
      if (newFilm[key] === '') {
        isValid = false;
        break;
      }

      newFilm[key] = newFilm[key].trim();
    }

    if (!isValid) return;

    newFilm = new Film(
      newFilm.id,
      newFilm.name,
      newFilm.country,
      newFilm.releaseDate,
      newFilm.duration,
      newFilm.director
    );
    // this.putRequest(newFilm as Film).subscribe(() => {
    //   this.refreshData();
    // });
  }
}
