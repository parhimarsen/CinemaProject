import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { BehaviorSubject, forkJoin, Observable, of, zip } from 'rxjs';
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
      .pipe(catchError(this.handleError<Session[]>('getServices', [])));
  }

  private postRequest(session: any): Observable<Session> {
    return this.http
      .post<Session>(this.url, session, this.httpOptions)
      .pipe(catchError(this.handleError<Session>('addSession')));
  }

  private deleteRequest(id: string): Observable<Session> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<Session>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Session>('deleteSession')));
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
      .map((session, sessionIndex) => {
        let filmName = this.films.find((film) => {
          return film.id === session.filmId;
        })?.name;
        let hallOfSession = halls.find((hall) => {
          return hall.id === session.hallId;
        });
        let cinemaOfSession = this.cinemas.find((cinema) => {
          return cinema.id === hallOfSession.cinemaId;
        })!;
        return new SessionView(
          sessionIndex + 1,
          new Intl.NumberFormat('fr-BR', {
            style: 'currency',
            currency: 'BYN',
          }).format(parseFloat(session.cost.toFixed(2))),
          this.formatDate(session.showStart),
          this.formatDate(session.showEnd),
          cinemaOfSession?.name,
          hallOfSession.name,
          filmName!,
          session.id
        );
      })
      .sort((a, b) => a.id - b.id);
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
          },
        },
      };
      this.source = new LocalDataSource(this.sessionsView);
      this.isComplited.next(true);
    });
  }

  add(event: any): void {
    let newSession = event.newData;
    let isValid = true;

    for (let key in newSession) {
      if (key === 'id' || key === 'showEnd') continue;
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
    let addableTypeOfSeat = {
      showStart: newSession.showStart,
      hallId: addableHall?.id,
      filmId: addableFilm?.id,
      cost: parseFloat(newSession.cost.replace(',', '.')),
    };
    this.postRequest(addableTypeOfSeat).subscribe(() => this.refreshData());
  }

  delete(event: any): void {
    let session = event.data;

    this.deleteRequest(session.guidId).subscribe(() => {
      this.refreshData();
    });
  }

  edit(event: any): void {
    let newSession = event.newData;
    let isValid = true;

    for (let key in newSession) {
      if (key === 'id' || key === 'showEnd') continue;
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
    let editableTypeOfSeat = new Session(
      newSession.guidId,
      parseFloat(newSession.cost.replace(',', '.')),
      newSession.showStart,
      newSession.showStart,
      editableHall?.id!,
      editableFilm?.id!
    );
    this.putRequest(editableTypeOfSeat).subscribe(() => this.refreshData());
  }
}
