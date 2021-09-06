import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Login } from 'src/app/Models/login';
import { Registration } from 'src/app/Models/registration';
import { Token } from 'src/app/Models/token';
import { environment } from 'src/environments/environment';

export const REFRESH_TOKEN_KEY = 'refresh_token';
export const ACCESS_TOKEN_KEY = 'access_token';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient, private router: Router) {}

  register(registrationData: Registration): Observable<Token> {
    return this.http
      .post<Token>(
        `${environment.AUTH_URL}/register`,

        registrationData,
        this.httpOptions
      )
      .pipe(
        map((response) => {
          localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
          localStorage.setItem(ACCESS_TOKEN_KEY, response.jwtToken);
          this.startRefreshTokenTimer();
          return response;
        })
      );
  }

  login(loginData: Login): Observable<Token> {
    return this.http
      .post<Token>(
        `${environment.AUTH_URL}/authenticate`,
        loginData,
        this.httpOptions
      )
      .pipe(
        map((response) => {
          localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
          localStorage.setItem(ACCESS_TOKEN_KEY, response.jwtToken);
          this.startRefreshTokenTimer();
          return response;
        })
      );
  }

  logout() {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
    this.http
      .post(
        `${environment.AUTH_URL}/revoke-token`,
        { jwtToken: refreshToken },
        this.httpOptions
      )
      .subscribe();
    this.stopRefreshTokenTimer();
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    this.router.navigate(['/auth']);
  }

  refreshToken(): Observable<Token> {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
    return this.http
      .post<Token>(
        `${environment.AUTH_URL}/refresh-token`,
        { jwtToken: refreshToken },
        this.httpOptions
      )
      .pipe(
        map((response) => {
          localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
          localStorage.setItem(ACCESS_TOKEN_KEY, response.jwtToken);
          this.startRefreshTokenTimer();
          return response;
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
