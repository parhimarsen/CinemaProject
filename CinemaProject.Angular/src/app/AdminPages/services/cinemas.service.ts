import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { Observable, of, BehaviorSubject, forkJoin } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CitiesService } from './cities.service';
import { SelectComponent } from '../MainPage/custom/select/select.component';
import { InputComponent } from '../MainPage/custom/input/input.component';
import { SelectEditComponent } from '../MainPage/custom/select-edit/select-edit.component';

import { Cinema, CinemaView } from '../Models/cinema';
import { City } from '../Models/city';
import { HallView } from '../Models/hall';
import { TypeOfSeat } from '../Models/typeOfSeat';

@Injectable({
  providedIn: 'root',
})
export class CinemasService {
  url = 'https://localhost:44356/api/Cinemas';

  //ng2-smart-table properties
  source: LocalDataSource;
  settings: any;

  isComplited = new BehaviorSubject<boolean>(false);

  cinemas: Cinema[];
  cities: City[];
  cinemasView: CinemaView[];

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient, private citiesService: CitiesService) {
    this.cinemas = [];
    this.cities = [];
    this.cinemasView = [];
    this.source = new LocalDataSource();
  }

  private handleError<Cinema>(operation = 'operation', result?: Cinema) {
    return (error: any): Observable<Cinema> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as Cinema);
    };
  }

  getAll(): Observable<Cinema[]> {
    return this.http
      .get<Cinema[]>(this.url)
      .pipe(catchError(this.handleError<Cinema[]>('getCinemas', [])));
  }

  private postRequest(cinema: Cinema): Observable<Cinema> {
    return this.http
      .post<Cinema>(this.url, cinema, this.httpOptions)
      .pipe(catchError(this.handleError<Cinema>('addCinema')));
  }

  private deleteRequest(id: string): Observable<Cinema> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<Cinema>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Cinema>('deleteCinema')));
  }

  private putRequest(cinema: Cinema): Observable<any> {
    return this.http
      .put(this.url, cinema, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateCinema')));
  }

  //CinemaView get cityName and Id as Number not Guid
  //Display priority depends on Id and not on Guid
  private convertCinemasToView(cinemas: Cinema[]): CinemaView[] {
    return cinemas
      .map((cinema, cinemaIndex) => {
        let cityName = this.cities.find((city) => {
          return city.id === cinema.cityId;
        })?.name;
        return new CinemaView(
          cinemaIndex + 1,
          cinema.name,
          cinema
            .halls!.sort((a, b) => parseInt(a.name) - parseInt(b.name))
            .map((hall, hallIndex) => {
              return new HallView(
                hallIndex + 1,
                hall.name,
                cinema.id,
                cinema.name,
                hall.id
              );
            }),
          cinema.id,
          cityName
        );
      })
      .sort((a, b) => a.id - b.id);
  }

  refreshData() {
    //Need to initialize cities earlier then cinemas
    forkJoin({
      cinemas: this.getAll(),
      cities: this.citiesService.getAll(),
    }).subscribe((response) => {
      this.cities = response.cities;
      this.cinemas = response.cinemas;
      console.log(this.cinemas);
      this.cinemasView = this.convertCinemasToView(this.cinemas);
      /* 
        Need to Get cities to set drop-down list:
        columns => cityName => editor => list.
        Halls presented custom SelectComponent
        valuePrepareFunction property to transfer data in this component
      */
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
          cityName: {
            title: 'City',
            filter: false,
            editor: {
              type: 'custom',
              component: SelectEditComponent,
            },
          },
          halls: {
            title: 'Halls',
            filter: false,
            addable: false,
            editable: false,
            type: 'custom',
            valuePrepareFunction: (halls: HallView[]) => halls,
            renderComponent: SelectComponent,
            editor: {
              type: 'list',
            },
          },
        },
      };
      this.source = new LocalDataSource(this.cinemasView);
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
            field: 'id',
            search: query,
          },
          {
            field: 'name',
            search: query,
          },
          {
            field: 'cityName',
            search: query,
          },
        ],
        false
      );
    }
  }

  add(event: any): void {
    let cinema = event.newData;
    let isValid = true;

    for (let key in cinema) {
      if (key === 'id' || key === 'halls') continue;
      if (cinema[key] === '') {
        isValid = false;
        break;
      }
      cinema[key] = cinema[key].trim();
    }

    if (!isValid) return;

    let cityName: string = cinema.cityName!;
    let cityId = this.cities.find((city) => city.name === cityName)?.id;

    if (cityId !== undefined) {
      let newCinema = { name: cinema.name, cityId: cityId };
      if (!newCinema) return;
      this.postRequest(newCinema as Cinema).subscribe((cinema) => {
        let newTypeOfSeat = {
          cinemaId: cinema.id,
          name: 'Common',
          extraPaymentPercent: 0,
        };
        this.http
          .post<TypeOfSeat>(
            'https://localhost:44356/api/TypesOfSeat',
            newTypeOfSeat,
            this.httpOptions
          )
          .subscribe(() => {
            this.refreshData();
          });
      });
    }
  }

  delete(event: any): void {
    let deletedCinema: any = this.cinemas.find(
      (cinema) => cinema.id === event.data.guidId
    )!;
    console.log(deletedCinema);
    this.http
      .post<TypeOfSeat>(
        `${this.url}/${deletedCinema.id}/TypesOfSeatRange`,
        deletedCinema.typesOfSeat,
        this.httpOptions
      )
      .subscribe(() => {
        this.deleteRequest(deletedCinema.id).subscribe(() => {
          this.refreshData();
        });
      });
  }

  edit(event: any): void {
    let newCinema = event.newData;

    let isValid = true;

    for (let key in newCinema) {
      if (key === 'id' || key === 'halls') continue;
      if (newCinema[key] === '') {
        isValid = false;
        break;
      }
      newCinema[key] = newCinema[key].trim();
    }

    if (!isValid) return;

    let cityId = this.cities.find((city) => {
      return city.name === newCinema.cityName;
    })?.id;

    if (cityId !== undefined) {
      newCinema = new Cinema(newCinema.guidId, newCinema.name, cityId, null);
      this.putRequest(newCinema).subscribe(() => {
        this.refreshData();
      });
    }
  }
}
