import { Injectable } from '@angular/core';

import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';

import { City } from '../Models/city';
import { LocalDataSource } from 'ng2-smart-table';
import { Ng2InputComponent } from '../MainPage/custom/input/ng2-input.component';
import { InputValidator } from '../MainPage/custom/validators/input-validator';

@Injectable({
  providedIn: 'root',
})
export class CitiesService {
  url = 'https://localhost:44356/api/Cities';

  source: LocalDataSource;
  settings: any;

  isComplited = new BehaviorSubject<boolean>(false);

  cities: City[];

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {
    this.cities = [];
    this.source = new LocalDataSource();
  }

  private handleError<City>(operation = 'operation', result?: City) {
    return (error: any): Observable<City> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as City);
    };
  }

  getAll(): Observable<City[]> {
    return this.http
      .get<City[]>(this.url)
      .pipe(catchError(this.handleError<City[]>('getCities', [])));
  }

  postRequest(city: City): Observable<City> {
    return this.http
      .post<City>(this.url, city, this.httpOptions)
      .pipe(catchError(this.handleError<City>('addCity')));
  }

  deleteRequest(id: string): Observable<City> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<City>(url, this.httpOptions)
      .pipe(catchError(this.handleError<City>('deleteCity')));
  }

  putRequest(city: City): Observable<any> {
    return this.http
      .put(this.url, city, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateCity')));
  }

  refreshData() {
    this.getAll().subscribe((cities) => {
      this.cities = cities;

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
        },
      };

      this.source = new LocalDataSource(this.cities);
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
        ],
        false
      );
    }
  }

  add(event: any): void {
    let newCity = event.newData;
    let isValid = true;

    for (let field in InputValidator.isNumbersValid) {
      if (!InputValidator.isNumbersValid[field]) return;
    }
    for (let field in InputValidator.isSpacesValid) {
      if (!InputValidator.isSpacesValid[field]) return;
    }

    for (let key in newCity) {
      if (newCity[key] === '') {
        isValid = false;
        break;
      }
      newCity[key] = newCity[key].trim();
    }

    if (!isValid) return;

    this.postRequest(newCity as City).subscribe(() => {
      this.refreshData();
    });
  }

  delete(event: any): void {
    let newCity = event.data;

    this.deleteRequest(newCity.id).subscribe(() => {
      this.refreshData();
    });
  }

  edit(event: any): void {
    let newHall = event.newData;
    let isValid = true;

    for (let field in InputValidator.isNumbersValid) {
      if (!InputValidator.isNumbersValid[field]) return;
    }
    for (let field in InputValidator.isSpacesValid) {
      if (!InputValidator.isSpacesValid[field]) return;
    }

    for (let key in newHall) {
      if (newHall[key] === '') {
        isValid = false;
        break;
      }
      newHall[key] = newHall[key].trim();
    }

    if (!isValid) return;

    this.putRequest(newHall).subscribe(() => {
      this.refreshData();
    });
  }
}
