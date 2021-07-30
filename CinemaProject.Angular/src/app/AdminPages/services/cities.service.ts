import { Injectable } from '@angular/core';

import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';

import { City } from '../Models/city';

@Injectable({
  providedIn: 'root',
})
export class CitiesService {
  url = 'https://localhost:44356/api/Cities';

  data = new BehaviorSubject<City[]>([]);

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {}

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }

  getAll() {
    this.http
      .get<City[]>(this.url)
      .pipe(catchError(this.handleError<City[]>('getCities', [])))
      .subscribe((cities) => {
        this.data.next(cities);
      });
  }

  add(city: City): Observable<City> {
    return this.http
      .post<City>(this.url, city, this.httpOptions)
      .pipe(catchError(this.handleError<City>('addCity')));
  }

  delete(id: string): Observable<City> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<City>(url, this.httpOptions)
      .pipe(catchError(this.handleError<City>('deleteCity')));
  }

  update(city: City): Observable<any> {
    return this.http
      .put(this.url, city, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateCity')));
  }
}
