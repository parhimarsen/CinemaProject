import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { Observable, of, BehaviorSubject, forkJoin } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CinemasService } from './cinemas.service';

import { Hall, HallView } from '../Models/hall';
import { Cinema } from '../Models/cinema';
import { InputComponent } from '../MainPage/custom/input/input.component';
import { SelectEditComponent } from '../MainPage/custom/select-edit/select-edit.component';
import { Seat } from '../Models/seat';

@Injectable({
  providedIn: 'root',
})
export class HallsService {
  url = 'https://localhost:44356/api/Halls';

  //Properties of ng2-smart-table
  source: LocalDataSource;
  settings: any;

  //For checking if loading async information complited
  isComplited = new BehaviorSubject<boolean>(false);

  //Data of this table
  halls: Hall[];
  //Halls are adding to cinemas
  //Need information in drop-down list
  cinemas: Cinema[];
  //Info in table is not same
  hallsView: HallView[];

  //Options for request
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(
    private http: HttpClient,
    private cinemasService: CinemasService
  ) {
    this.halls = [];
    this.cinemas = [];
    this.hallsView = [];
    this.source = new LocalDataSource();
  }

  private handleError<Cinema>(operation = 'operation', result?: Cinema) {
    return (error: any): Observable<Cinema> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as Cinema);
    };
  }

  private getAll(): Observable<Hall[]> {
    return this.http
      .get<Hall[]>(this.url)
      .pipe(catchError(this.handleError<Hall[]>('getHalls', [])));
  }

  //Method used in TypesOfSeatService for providing Seats of Hall
  //Needed to change Types of Seats, when Type was deleted
  getSeats(id: string): Observable<Seat[]> {
    return this.http
      .get<Seat[]>(`${this.url}/${id}/Seats`)
      .pipe(catchError(this.handleError<Seat[]>('getSeatsOfHall', [])));
  }

  private postRequest(hall: Hall): Observable<Hall> {
    return this.http
      .post<Hall>(this.url, hall, this.httpOptions)
      .pipe(catchError(this.handleError<Hall>('addHall')));
  }

  postSeatsRangeRequest(
    hallId: string,
    seatsRange: Seat[]
  ): Observable<Seat[]> {
    return this.http.post<Seat[]>(
      `${this.url}/${hallId}/Seats`,
      seatsRange,
      this.httpOptions
    );
  }

  private deleteRequest(id: string): Observable<Hall> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<Hall>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Hall>('deleteHall')));
  }

  deleteSeatsRangeRequest(
    hallId: string,
    seatsRange: Seat[]
  ): Observable<Seat[]> {
    return this.http.post<Seat[]>(
      `${this.url}/${hallId}/SeatsRange`,
      seatsRange,
      this.httpOptions
    );
  }

  private putRequest(hall: Hall): Observable<any> {
    return this.http
      .put(this.url, hall, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateHall')));
  }

  putSeatsRangeRequest(hallId: string, seatsRange: Seat[]): Observable<any> {
    return this.http.put(
      `${this.url}/${hallId}/Seats`,
      seatsRange,
      this.httpOptions
    );
  }

  //Halls are sorted by Cinema's names
  private convertHallsToView(halls: Hall[], cinemas: Cinema[]): HallView[] {
    return halls
      .map((hall) => {
        let cinemaName = cinemas.find(
          (cinema) => cinema.id === hall.cinemaId
        )?.name;
        return new HallView(hall.id, hall.name, hall.id, cinemaName!);
      })
      .sort((a, b) => {
        if (a.cinemaName > b.cinemaName) {
          return 1;
        }
        if (a.cinemaName < b.cinemaName) {
          return -1;
        }
        if (a.id > b.id) {
          return 1;
        }
        if (a.id < b.id) {
          return -1;
        }
        return 0;
      });
  }

  refreshData() {
    forkJoin({
      halls: this.getAll(),
      cinemas: this.cinemasService.getAll(),
    }).subscribe((response) => {
      this.halls = response.halls;
      this.cinemas = response.cinemas;
      this.hallsView = this.convertHallsToView(this.halls, this.cinemas);

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
              component: InputComponent,
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
        },
      };
      this.source = new LocalDataSource(this.hallsView);
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
            field: 'name',
            search: query,
          },
          {
            field: 'cinemaName',
            search: query,
          },
        ],
        false
      );
    }
  }

  add(event: any): void {
    let hall = event.newData;
    //For checking if inputs are required
    //Cant Validate in different way => the solution of ng2-smart-table validation is too complicated
    let isValid = true;

    for (let key in hall) {
      if (hall[key] === '') {
        isValid = false;
        break;
      }
      hall[key] = hall[key].trim();
    }

    if (!isValid) return;

    //Get id of selected Cinema by Name. Need to Resolve this Problem
    let cinemaName: string = hall.cinemaName!;
    let cinemaId = this.cinemas.find(
      (cinema) => cinema.name === cinemaName
    )?.id;

    if (cinemaId !== undefined) {
      let newHall = { name: hall.name, cinemaId: cinemaId };
      if (!newHall) return;
      this.postRequest(newHall as Hall).subscribe(() => {
        this.refreshData();
      });
    }
  }

  delete(event: any): void {
    let hall = event.data;

    this.deleteRequest(hall.id).subscribe(() => {
      this.refreshData();
    });
  }

  //Same as add method but with Id
  edit(event: any): void {
    let newHall = event.newData;
    let isValid = true;

    for (let key in newHall) {
      if (newHall[key] === '') {
        isValid = false;
        break;
      }
      newHall[key] = newHall[key].trim();
    }

    if (!isValid) return;

    let cinemaId = this.cinemas.find((cinema) => {
      return cinema.name === newHall.cinemaName;
    })?.id;

    if (cinemaId !== undefined) {
      newHall = new Hall(newHall.id, newHall.name, cinemaId);
      this.putRequest(newHall).subscribe(() => {
        this.refreshData();
      });
    }
  }
}
