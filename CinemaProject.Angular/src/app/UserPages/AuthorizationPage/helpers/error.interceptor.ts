import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthFormComponent } from '../auth-form/auth-form.component';
import {
  ACCESS_TOKEN_KEY,
  AuthService,
  REFRESH_TOKEN_KEY,
} from '../services/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((err) => {
        if (err.status === 400) {
          AuthFormComponent.isWrongLoginorPassword = true;
        }

        if (
          [401, 403].includes(err.status) &&
          localStorage.getItem(REFRESH_TOKEN_KEY) &&
          localStorage.getItem(ACCESS_TOKEN_KEY)
        ) {
          // auto logout if 401 or 403 response returned from api
          this.authenticationService.logout();
          AuthFormComponent.isSessionExpired = true;
        }

        const error = (err && err.error && err.error.message) || err.statusText;
        console.error(err);
        return throwError(error);
      })
    );
  }
}
