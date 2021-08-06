import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { Observable, of, BehaviorSubject, forkJoin } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CinemasService } from './cinemas.service';
import { HallsService } from './halls.service';

import { TypeOfSeat, TypeOfSeatView } from '../Models/typeOfSeat';
import { Cinema } from '../Models/cinema';
import { SelectEditComponent } from '../MainPage/custom/select-edit/select-edit.component';
import { SeatsService } from './seats.service';
import { InputComponent } from '../MainPage/custom/input/input.component';

@Injectable({
  providedIn: 'root',
})
export class TypesOfSeatService {
  url = 'https://localhost:44356/api/TypesOfSeat';

  source: LocalDataSource;
  settings: any;

  isComplited = new BehaviorSubject<boolean>(false);

  typesOfSeat: TypeOfSeat[];
  typesOfSeatView: TypeOfSeatView[];
  cinemas: Cinema[];
  flag: boolean = true;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(
    private http: HttpClient,
    private cinemasService: CinemasService,
    private hallsService: HallsService,
    private seatsService: SeatsService
  ) {
    this.typesOfSeat = [];
    this.typesOfSeatView = [];
    this.cinemas = [];
    this.source = new LocalDataSource();
  }

  private handleError<TypeOfSeat>(
    operation = 'operation',
    result?: TypeOfSeat
  ) {
    return (error: any): Observable<TypeOfSeat> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as TypeOfSeat);
    };
  }

  getAll(): Observable<TypeOfSeat[]> {
    return this.http
      .get<TypeOfSeat[]>(this.url)
      .pipe(catchError(this.handleError<TypeOfSeat[]>('getTypesOfSeat', [])));
  }

  private postRequest(typeOfSeat: any): Observable<TypeOfSeat> {
    return this.http
      .post<TypeOfSeat>(this.url, typeOfSeat, this.httpOptions)
      .pipe(catchError(this.handleError<any>('addTypeOfSeat')));
  }

  private deleteRequest(id: string): Observable<TypeOfSeat> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<TypeOfSeat>(url, this.httpOptions)
      .pipe(catchError(this.handleError<TypeOfSeat>('deleteTypeOfSeat')));
  }

  private putRequest(typeOfSeat: any): Observable<any> {
    return this.http
      .put(this.url, typeOfSeat, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateTypeOfSeat')));
  }

  private convertTypesOfSeatToView(
    typesOfSeat: TypeOfSeat[]
  ): TypeOfSeatView[] {
    return typesOfSeat
      .map((typeOfSeat) => {
        let cinemaName = this.cinemas.find(
          (cinema) => cinema.id === typeOfSeat.cinemaId
        )?.name;
        return new TypeOfSeatView(
          typeOfSeat.id,
          cinemaName!,
          typeOfSeat.name,
          typeOfSeat.extraPaymentPercent + ' %',
          ''
        );
      })
      .sort((a, b) => {
        if (a.cinemaName > b.cinemaName) {
          return 1;
        }
        if (a.cinemaName < b.cinemaName) {
          return -1;
        }
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
    forkJoin({
      typesOfSeat: this.getAll(),
      cinemas: this.cinemasService.getAll(),
    }).subscribe((response) => {
      this.typesOfSeat = response.typesOfSeat;
      this.cinemas = response.cinemas;
      this.typesOfSeatView = this.convertTypesOfSeatToView(this.typesOfSeat);

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
          cinemaName: {
            title: 'Cinema',
            filter: false,
            editor: {
              type: 'custom',
              component: SelectEditComponent,
            },
          },
          name: {
            title: 'Name',
            filter: false,
            editor: {
              type: 'custom',
              component: InputComponent,
            },
          },
          extraPaymentPercent: {
            title: 'Extra (%) of Session Cost',
            filter: false,
            editor: {
              type: 'custom',
              component: InputComponent,
            },
          },
        },
      };
      this.source = new LocalDataSource(this.typesOfSeatView);
      this.isComplited.next(true);
    });
  }

  add(event: any): void {
    let typeOfSeat = event.newData;
    let isValid = true;

    for (let key in typeOfSeat) {
      if (key === 'id') continue;
      if (typeOfSeat[key] === '') {
        isValid = false;
        break;
      }
      typeOfSeat[key] = typeOfSeat[key].trim();
    }

    if (!isValid) return;

    let cinema = this.cinemas.find(
      (cinema) => cinema.name === typeOfSeat.cinemaName
    );
    let newTypeOfSeat = {
      cinemaId: cinema!.id,
      name: typeOfSeat.name,
      extraPaymentPercent: typeOfSeat.extraPaymentPercent,
    };
    this.postRequest(newTypeOfSeat).subscribe(() => this.refreshData());
  }

  delete(event: any): void {
    let typeOfSeat = event.data;

    // All Seats of deleted TypeOfSeat update to Common
    if (this.flag) {
      this.isComplited.subscribe(() => {
        let cinema = this.cinemas.find(
          (cinema) => cinema.name === typeOfSeat.cinemaName
        );
        let commonTypeOfSeat = this.typesOfSeat.find(
          (typeOfSeat) =>
            typeOfSeat.cinemaId === cinema?.id && typeOfSeat.name === 'Common'
        );
        cinema?.halls?.forEach((hall) => {
          this.hallsService.getSeats(hall.id).subscribe((seats) => {
            seats.forEach((seat) => {
              if (seat.typeOfSeatId === null) {
                seat.typeOfSeatId = commonTypeOfSeat!.id;
                this.seatsService.putSeatRequest(seat).subscribe();
              }
            });
          });
        });
      });
      this.flag = false;
    }

    if (typeOfSeat.name !== 'Common') {
      this.deleteRequest(typeOfSeat.id).subscribe(() => {
        this.refreshData();
      });
    }
  }

  edit(event: any): void {
    let typeOfSeat = event.newData;
    console.log(typeOfSeat);
    let isValid = true;

    for (let key in typeOfSeat) {
      if (key === 'id' || key === 'color') continue;
      if (typeOfSeat[key] === '') {
        isValid = false;
        break;
      }
      typeOfSeat[key] = typeOfSeat[key].trim();
    }

    if (!isValid) return;

    let cinema = this.cinemas.find(
      (cinema) => cinema.name === typeOfSeat.cinemaName
    );
    let newTypeOfSeat = {
      id: typeOfSeat.id,
      cinemaId: cinema!.id,
      name: typeOfSeat.name,
      extraPaymentPercent: typeOfSeat.extraPaymentPercent.slice(0, -2),
    };
    this.putRequest(newTypeOfSeat).subscribe(() => this.refreshData());
  }
}
