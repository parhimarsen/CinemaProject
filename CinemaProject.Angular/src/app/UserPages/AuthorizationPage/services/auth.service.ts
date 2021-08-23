import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Token } from 'src/app/Models/token';

export const REFRESH_TOKEN_KEY = 'refresh_token';
export const ACCESS_TOKEN_KEY = 'access_token';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  url = 'https://localhost:44356/api/Users';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient, private router: Router) {}

  register(
    email: string,
    login: string,
    password: string,
    confirmPassword: string
  ): Observable<Token> {
    return this.http
      .post<any>(
        `${this.url}/register`,
        {
          email: email,
          login: login,
          password: password,
          confirmPassword: confirmPassword,
        },
        this.httpOptions
      )
      .pipe(
        map((response) => {
          localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
          localStorage.setItem(ACCESS_TOKEN_KEY, response.jwtToken);
          this.startRefreshTokenTimer();
          return response.user;
        })
      );
  }

  login(login: string, password: string): Observable<Token> {
    return this.http
      .post<any>(
        `${this.url}/authenticate`,
        {
          login: login,
          password: password,
        },
        this.httpOptions
      )
      .pipe(
        map((response) => {
          localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
          localStorage.setItem(ACCESS_TOKEN_KEY, response.jwtToken);
          this.startRefreshTokenTimer();
          return response.user;
        })
      );
  }

  logout() {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
    this.http
      .post<any>(
        `${this.url}/revoke-token`,
        { jwtToken: refreshToken },
        this.httpOptions
      )
      .subscribe();
    this.stopRefreshTokenTimer();
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    this.router.navigate(['/auth']);
  }

  refreshToken() {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
    return this.http
      .post<any>(
        `${this.url}/refresh-token`,
        { jwtToken: refreshToken },
        this.httpOptions
      )
      .pipe(
        map((response) => {
          localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
          localStorage.setItem(ACCESS_TOKEN_KEY, response.jwtToken);
          this.startRefreshTokenTimer();
          return response.user;
        })
      );
  }

  private refreshTokenTimeout: any;

  startRefreshTokenTimer() {
    // parse json object from base64 encoded jwt token
    const jwtToken = JSON.parse(
      atob(localStorage.getItem(ACCESS_TOKEN_KEY)!.split('.')[1])
    );

    // set a timeout to refresh the token a 5 sec before it expires
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - 5 * 1000;
    this.refreshTokenTimeout = setTimeout(
      () => this.refreshToken().subscribe(),
      timeout
    );
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
