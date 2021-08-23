import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';

import { ACCESS_TOKEN_KEY } from '../services/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  url = 'https://localhost:44356/api';

  constructor() {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const access_token = localStorage.getItem(ACCESS_TOKEN_KEY)!;
    const isLoggedIn = access_token;
    const isApiUrl = request.url.startsWith(this.url);
    if (isLoggedIn && isApiUrl) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${access_token}` },
      });
    }

    return next.handle(request);
  }
}
