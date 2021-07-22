import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Cinema } from './Models/cinema';
import { City } from './Models/city';

@Injectable({
  providedIn: 'root',
})
export class CinemasService {
  private cinemasUrl = 'https://localhost:44356/api/Cinemas';
  private citiesUrl = 'api/Cities';
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

  getCinemas(): Observable<Cinema[]> {
    return this.http
      .get<Cinema[]>(this.cinemasUrl)
      .pipe(catchError(this.handleError<Cinema[]>('getCinemas', [])));
  }

  getCitites(): Observable<City[]> {
    return this.http
      .get<City[]>(this.citiesUrl)
      .pipe(catchError(this.handleError<City[]>('getCities', [])));
  }

  getCityOfCinema(id: string): Observable<City> {
    const url = `${this.cinemasUrl}/${id}/City`;

    return this.http
      .get<City>(url)
      .pipe(catchError(this.handleError<City>('getCity', undefined)));
  }

  addCinema(cinema: Cinema): Observable<Cinema> {
    return this.http
      .post<Cinema>(this.cinemasUrl, cinema, this.httpOptions)
      .pipe(catchError(this.handleError<Cinema>('addCinema')));
  }

  deleteCinema(id: string): Observable<Cinema> {
    const url = `${this.cinemasUrl}/${id}`;

    return this.http
      .delete<Cinema>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Cinema>('deleteCinema')));
  }
}
