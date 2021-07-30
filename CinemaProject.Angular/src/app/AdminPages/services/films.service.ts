import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DatePickerComponent } from '../MainPage/custom/date-picker/date-picker.component';
import { InputComponent } from '../MainPage/custom/input/input.component';
import { DurationPickerComponent } from '../MainPage/custom/duration-picker/duration-picker.component';

import { Film, FilmView } from '../Models/film';

@Injectable({
  providedIn: 'root',
})
export class FilmsService {
  url = 'https://localhost:44356/api/Films';

  data = new BehaviorSubject<Film[]>([]);
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

  private getAll() {
    this.http
      .get<Film[]>(this.url)
      .pipe(catchError(this.handleError<Film[]>('getFilms', [])))
      .subscribe((films) => {
        this.data.next(films);
      });
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
      .map((film, index) => {
        return new FilmView(
          index + 1,
          film.name,
          film.country,
          this.formatDate(film.releaseDate),
          film.duration,
          film.director,
          film.id
        );
      })
      .sort((a, b) => a.id - b.id);
  }

  refreshData() {
    this.data.subscribe((films) => {
      this.films = films;
      this.filmsView = this.convertFilmToView(this.films);
      this.source = new LocalDataSource(this.filmsView);
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
        id: {
          title: 'ID',
          filter: false,
          addable: false,
          editable: false,
        },
        name: {
          title: 'Name',
          filter: false,
          editor: {
            type: 'custom',
            component: InputComponent,
          },
        },
        country: {
          title: 'Country',
          filter: false,
          editor: {
            type: 'custom',
            component: InputComponent,
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
            component: InputComponent,
          },
        },
      },
    };
    this.getAll();
  }

  onSearch(query: string) {
    if (query === '') {
      this.source.setFilter([]);
    } else {
      this.source.setFilter(
        [
          {
            field: 'id',
            search: query,
          },
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

    for (let key in newFilm) {
      if (newFilm[key] !== null) newFilm[key] = newFilm[key].trim();
    }

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

    this.deleteRequest(film.guidId).subscribe(() => {
      this.refreshData();
    });
  }

  edit(event: any): void {
    let newFilm = event.newData;

    newFilm = new Film(
      newFilm.guidId,
      newFilm.name,
      newFilm.country,
      newFilm.releaseDate,
      newFilm.duration,
      newFilm.director
    );
    this.putRequest(newFilm).subscribe(() => {
      this.refreshData();
    });
  }
}
