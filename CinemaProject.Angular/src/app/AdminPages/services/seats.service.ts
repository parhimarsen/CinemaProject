import { Injectable } from '@angular/core';

import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Seat } from '../Models/seat';

@Injectable({
  providedIn: 'root',
})
export class SeatsService {
  url = 'https://localhost:44356/api/Seats';

  data = new BehaviorSubject<Seat[]>([]);

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {}

  private handleError<Seat>(operation = 'operation', result?: Seat) {
    return (error: any): Observable<Seat> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as Seat);
    };
  }

  getAll(): Observable<Seat[]> {
    return this.http
      .get<Seat[]>(this.url)
      .pipe(catchError(this.handleError<Seat[]>('getSeats', [])));
  }

  add(seat: Seat): Observable<Seat> {
    return this.http
      .post<Seat>(this.url, seat, this.httpOptions)
      .pipe(catchError(this.handleError<Seat>('addSeat')));
  }

  delete(id: string): Observable<Seat> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<Seat>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Seat>('deleteSeat')));
  }

  update(seat: Seat): Observable<any> {
    return this.http
      .put(this.url, seat, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateSeat')));
  }
}
