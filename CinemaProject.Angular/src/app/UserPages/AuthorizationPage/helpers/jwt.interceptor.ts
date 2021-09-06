import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';

import { ACCESS_TOKEN_KEY } from '../services/auth.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const access_token = localStorage.getItem(ACCESS_TOKEN_KEY)!;
    const isApiUrl = request.url.startsWith(environment.API_URL);
    if (access_token && isApiUrl) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${access_token}` },
      });
    }

    return next.handle(request);
  }
}
