import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { BehaviorSubject, forkJoin, Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CurrencyInputComponent } from '../MainPage/custom/currency-input/currency-input.component';
import { SelectEditComponent } from '../MainPage/custom/select-edit/select-edit.component';
import { DateTimePickerComponent } from '../MainPage/custom/date-time-picker/date-time-picker.component';
import { CinemasService } from './cinemas.service';
import { FilmsService } from './films.service';

import { Session, SessionView } from '../Models/session';
import { Film } from '../Models/film';
import { Cinema } from '../Models/cinema';
import { Amenity, AmenityView } from '../Models/amenity';
import { EditableSelectComponent } from '../MainPage/custom/editable-select/editable-select.component';
import { DateTimeValidator } from '../MainPage/custom/validators/date-time-validator';

@Injectable({
  providedIn: 'root',
})
export class SessionsService {
  url = 'https://localhost:44356/api/Sessions';

  source: LocalDataSource;
  settings: any;

  isComplited = new BehaviorSubject<boolean>(false);

  sessions: Session[];
  sessionsView: SessionView[];
  films: Film[];
  cinemas: Cinema[];

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(
    private http: HttpClient,
    private cinemasService: CinemasService,
    private filmsService: FilmsService
  ) {
    this.sessions = [];
    this.sessionsView = [];
    this.films = [];
    this.cinemas = [];
    this.source = new LocalDataSource();
  }

  private handleError<Session>(operation = 'operation', result?: Session) {
    return (error: any): Observable<Session> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as Session);
    };
  }

  private getAll(): Observable<Session[]> {
    return this.http
      .get<Session[]>(this.url)
      .pipe(catchError(this.handleError<Session[]>('getSessions', [])));
  }

  private getAmenities(id: string): Observable<Amenity[]> {
    return this.http
      .get<Amenity[]>(`${this.url}/${id}/Amenities`)
      .pipe(catchError(this.handleError<Amenity[]>('getAmenities', [])));
  }

  private postRequest(session: any): Observable<Session> {
    return this.http
      .post<Session>(this.url, session, this.httpOptions)
      .pipe(catchError(this.handleError<Session>('addSession')));
  }

  postAmenity(id: String, amentity: Amenity): Observable<any> {
    return this.http
      .post(`${this.url}/${id}/Amenities`, amentity, this.httpOptions)
      .pipe(catchError(this.handleError<any>('addAmenity')));
  }

  private deleteRequest(id: string): Observable<Session> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<Session>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Session>('deleteSession')));
  }

  deleteAmenity(id: string, amenityId: string): Observable<any> {
    const url = `${this.url}/${id}/Amenities/${amenityId}`;

    return this.http
      .delete<any>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Session>('deleteAmenity')));
  }

  private putRequest(session: Session): Observable<any> {
    return this.http
      .put(this.url, session, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateSession')));
  }

  private convertSessionsToView(sessions: Session[]): SessionView[] {
    let halls: any[] = [];
    this.cinemas.forEach((cinema) => {
      cinema.halls?.forEach((hall) => {
        halls.push(hall);
      });
    });
    return sessions
      .map((session) => {
        let filmName = this.films.find((film) => {
          return film.id === session.filmId;
        })?.name;
        let hallOfSession = halls.find((hall) => {
          return hall.id === session.hallId;
        });
        let cinemaOfSession = undefined;
        if (hallOfSession) {
          cinemaOfSession = this.cinemas.find((cinema) => {
            return cinema.id === hallOfSession.cinemaId;
          })!;
        }
        return new SessionView(
          session.id,
          new Intl.NumberFormat('fr-BR', {
            style: 'currency',
            currency: 'BYN',
          }).format(parseFloat(session.cost.toFixed(2))),
          this.formatDate(session.showStart),
          this.formatDate(session.showEnd),
          cinemaOfSession ? cinemaOfSession?.name : session.cinemaName,
          hallOfSession ? hallOfSession.name : session.hallName,
          filmName ? filmName! : session.filmName,
          cinemaOfSession && filmName
            ? this.generateStatus(session)
            : 'Not Exist',
          []
        );
      })
      .sort((a, b) => {
        if (a.showStart < b.showStart) {
          return 1;
        }
        if (a.showStart > b.showStart) {
          return -1;
        }
        return 0;
      });
  }

  private generateStatus(session: Session): string {
    let now = new Date();
    return now > new Date(session.showEnd)
      ? 'Ended'
      : now > new Date(session.showStart)
      ? '!Online!'
      : 'Active';
  }

  private formatDate(date: Date): string {
    let newDate =
      date.toString().slice(0, 10).split('-').reverse().join('-') +
      `\n` +
      date.toString().slice(11, 19);
    return newDate;
  }

  refreshData() {
    forkJoin({
      sessions: this.getAll(),
      films: this.filmsService.getAll(),
      cinemas: this.cinemasService.getAll(),
    }).subscribe((response) => {
      this.sessions = response.sessions;
      this.cinemas = response.cinemas;
      this.films = response.films;

      this.sessionsView = this.convertSessionsToView(this.sessions);
      this.sessionsView.forEach((sessionView) => {
        this.getAmenities(sessionView.id).subscribe((amenitiesOfSession) => {
          sessionView.affortableAmenities = amenitiesOfSession.map(
            (amenityOfSession) => {
              return new AmenityView(
                amenityOfSession.id,
                amenityOfSession.name,
                new Intl.NumberFormat('fr-BR', {
                  style: 'currency',
                  currency: 'BYN',
                }).format(parseFloat(amenityOfSession.cost.toFixed(2))),
                amenityOfSession.extraPaymentPercent,
                sessionView.id
              );
            }
          );
        });
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
          filmName: {
            title: 'Film',
            filter: false,
            editor: {
              type: 'custom',
              component: SelectEditComponent,
            },
          },
          cinemaName: {
            title: 'Cinema',
            filter: false,
            editor: {
              type: 'custom',
              component: SelectEditComponent,
            },
          },
          hallName: {
            title: 'Hall',
            filter: false,
            editor: {
              type: 'custom',
              component: SelectEditComponent,
            },
          },
          cost: {
            title: 'Cost',
            filter: false,
            editor: {
              type: 'custom',
              component: CurrencyInputComponent,
            },
          },
          showStart: {
            title: 'Show Start',
            filter: false,
            editable: false,
            editor: {
              type: 'custom',
              component: DateTimePickerComponent,
            },
          },
          showEnd: {
            title: 'Show End',
            filter: false,
            addable: false,
            editable: false,
            editor: {
              type: 'custom',
              component: DateTimePickerComponent,
            },
          },
          affortableAmenities: {
            title: 'Services',
            filter: false,
            addable: false,
            editable: false,
            type: 'custom',
            valuePrepareFunction: (
              affortableAmenities: AmenityView[],
              session: SessionView
            ) => [session],
            renderComponent: EditableSelectComponent,
            editor: {
              type: 'list',
            },
          },
          status: {
            title: 'Status',
            filter: false,
            addable: false,
            editable: false,
          },
        },
      };
      this.source = new LocalDataSource(this.sessionsView);
      this.isComplited.next(true);
    });
  }

  onSearch(query: string) {
    if (query === '') {
      this.source.setFilter([]);
    } else {
      this.source.setFilter(
        [
          {
            field: 'cost',
            search: query,
          },
          {
            field: 'hallName',
            search: query,
          },
          {
            field: 'cinemaName',
            search: query,
          },
          {
            field: 'filmName',
            search: query,
          },
          {
            field: 'showStart',
            search: query,
          },
          {
            field: 'status',
            search: query,
          },
        ],
        false
      );
    }
  }

  add(event: any): void {
    let newSession = event.newData;
    let isValid = true;

    for (let field in DateTimeValidator.isDateValid) {
      if (!DateTimeValidator.isDateValid[field]) return;
    }

    for (let key in newSession) {
      if (
        key === 'showEnd' ||
        key === 'status' ||
        key === 'affortableAmenities'
      )
        continue;
      if (newSession[key] === '') {
        isValid = false;
        break;
      }
      newSession[key] = newSession[key].trim();
    }

    if (!isValid) return;

    let addableFilm = this.films.find(
      (film) => film.name === newSession.filmName
    );
    let addableCinema = this.cinemas.find(
      (cinema) => cinema.name === newSession.cinemaName
    );
    let addableHall = addableCinema!.halls!.find(
      (hall) => hall.name === newSession.hallName
    );
    newSession = {
      showStart: newSession.showStart,
      hallId: addableHall?.id,
      filmId: addableFilm?.id,
      cost: parseFloat(newSession.cost.replace(',', '.')),
      cinemaName: addableCinema?.name,
      hallName: addableHall?.name,
      filmName: addableFilm?.name,
    };
    this.postRequest(newSession).subscribe(() => this.refreshData());
  }

  delete(event: any): void {
    let session = event.data;

    this.deleteRequest(session.id).subscribe(() => {
      this.refreshData();
    });
  }

  edit(event: any): void {
    let newSession = event.newData;
    let isValid = true;

    for (let key in newSession) {
      if (
        key === 'showEnd' ||
        key === 'status' ||
        key === 'affortableAmenities'
      )
        continue;
      if (newSession[key] === '') {
        isValid = false;
        break;
      }
      newSession[key] = newSession[key].trim();
    }

    if (!isValid) return;

    let editableFilm = this.films.find(
      (film) => film.name === newSession.filmName
    );
    let editableCinema = this.cinemas.find(
      (cinema) => cinema.name === newSession.cinemaName
    );
    let editableHall = editableCinema!.halls!.find(
      (hall) => hall.name === newSession.hallName
    );
    newSession = new Session(
      newSession.id,
      parseFloat(newSession.cost.replace(',', '.')),
      newSession.showStart,
      newSession.showStart,
      editableHall?.id!,
      editableCinema?.name!,
      editableHall?.name!,
      editableFilm?.name!,
      editableFilm?.id!,
      []
    );
    this.putRequest(newSession).subscribe(() => this.refreshData());
  }
}
