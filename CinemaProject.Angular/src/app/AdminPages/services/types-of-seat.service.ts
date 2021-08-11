import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';

import { TypeOfSeat } from '../Models/typeOfSeat';

@Injectable({
  providedIn: 'root',
})
export class TypesOfSeatService {
  url = 'https://localhost:44356/api/TypesOfSeat';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {}

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

  postRequest(typeOfSeat: any): Observable<TypeOfSeat> {
    return this.http
      .post<TypeOfSeat>(this.url, typeOfSeat, this.httpOptions)
      .pipe(catchError(this.handleError<any>('addTypeOfSeat')));
  }

  deleteRequest(id: string): Observable<TypeOfSeat> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<TypeOfSeat>(url, this.httpOptions)
      .pipe(catchError(this.handleError<TypeOfSeat>('deleteTypeOfSeat')));
  }

  putRequest(typeOfSeat: any): Observable<any> {
    return this.http
      .put(this.url, typeOfSeat, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateTypeOfSeat')));
  }
}
