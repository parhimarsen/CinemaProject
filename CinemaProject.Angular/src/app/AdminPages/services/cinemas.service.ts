import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { Observable, of, BehaviorSubject } from 'rxjs';
import { zip } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CitiesService } from './cities.service';
import { SelectComponent } from '../MainPage/custom/select/select.component';

import { Cinema, CinemaView } from '../Models/cinema';
import { City } from '../Models/city';
import { HallView } from '../Models/hall';

@Injectable({
  providedIn: 'root',
})
export class CinemasService {
  url = 'https://localhost:44356/api/Cinemas';

  //ng2-smart-table properties
  source: LocalDataSource;
  settings: any;

  data = new BehaviorSubject<Cinema[]>([]);

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

  private getAll() {
    this.http
      .get<Cinema[]>(this.url)
      .pipe(catchError(this.handleError<Cinema[]>('getCinemas', [])))
      .subscribe((cinemas) => {
        this.data.next(cinemas);
      });
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
              return new HallView(hallIndex + 1, hall.name, cinema.id, hall.id);
            }),
          cinema.id,
          cityName
        );
      })
      .sort((a, b) => a.id - b.id);
  }

  refreshData() {
    //Need to initialize cities earlier then cinemas
    zip(this.data, this.citiesService.data).subscribe(([cinemas, cities]) => {
      this.cities = cities;
      this.cinemas = cinemas;
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
          },
          cityName: {
            title: 'City',
            filter: false,
            editor: {
              type: 'list',
              config: {
                list: this.cities.map((city) => {
                  return { value: city.name, title: city.name };
                }),
              },
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
    });

    this.citiesService.data.subscribe((cities: City[]) => {
      this.cities = cities;
    });
    this.citiesService.getAll();
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

    for (let key in cinema) {
      if (cinema[key] !== null) cinema[key] = cinema[key].trim();
    }

    let cityName: string = cinema.cityName!;
    let cityId = this.cities.find((city) => city.name === cityName)?.id;

    if (cityId !== undefined) {
      let newCinema = { name: cinema.name, cityId: cityId };
      if (!newCinema) return;
      this.postRequest(newCinema as Cinema).subscribe(() => {
        this.refreshData();
      });
    }
  }

  delete(event: any): void {
    let cinema = event.data;

    this.deleteRequest(cinema.guidId).subscribe(() => {
      this.refreshData();
    });
  }

  edit(event: any): void {
    let newCinema = event.newData;

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
