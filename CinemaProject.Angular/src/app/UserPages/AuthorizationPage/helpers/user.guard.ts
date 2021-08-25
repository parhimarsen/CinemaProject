import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ACCESS_TOKEN_KEY } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class UserGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate() {
    const token = localStorage.getItem(ACCESS_TOKEN_KEY);

    if (token) {
      const tokenPayload = JSON.parse(
        atob(localStorage.getItem(ACCESS_TOKEN_KEY)!.split('.')[1])
      );

      if (tokenPayload.isAdmin) {
        this.router.navigate(['/admin/cinemas']);
      }
      return true;
    } else {
      this.router.navigate(['/auth']);
      return false;
    }
  }
}
