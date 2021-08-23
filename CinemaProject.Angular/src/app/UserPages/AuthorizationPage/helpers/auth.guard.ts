import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ACCESS_TOKEN_KEY, AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate() {
    const token = localStorage.getItem(ACCESS_TOKEN_KEY);

    if (token) {
      const tokenPayload = JSON.parse(
        atob(localStorage.getItem(ACCESS_TOKEN_KEY)!.split('.')[1])
      );

      if (tokenPayload.admin === 'True') {
        this.router.navigate(['/admin/cinemas']);
      } else {
        this.router.navigate(['/user/main']);
      }
      return true;
    } else {
      return true;
    }
  }
}
