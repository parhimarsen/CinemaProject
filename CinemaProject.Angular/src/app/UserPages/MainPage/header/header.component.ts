import { Component, OnInit } from '@angular/core';
import {
  ACCESS_TOKEN_KEY,
  AuthService,
} from '../../AuthorizationPage/services/auth.service';

@Component({
  selector: 'app-user-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  userName!: string;

  constructor(private authenticationService: AuthService) {}

  ngOnInit(): void {
    const jwtToken = JSON.parse(
      atob(localStorage.getItem(ACCESS_TOKEN_KEY)!.split('.')[1])
    );
    this.userName = jwtToken.login;
  }

  logOut() {
    this.authenticationService.logout();
  }
}
