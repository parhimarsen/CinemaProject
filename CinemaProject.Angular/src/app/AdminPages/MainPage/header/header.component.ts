import {
  Component,
  AfterViewInit,
  ChangeDetectorRef,
  OnInit,
} from '@angular/core';
import {
  ACCESS_TOKEN_KEY,
  AuthService,
} from 'src/app/UserPages/AuthorizationPage/services/auth.service';
import { HeaderService } from '../../services/header.service';

@Component({
  selector: 'app-admin-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit, AfterViewInit {
  userName!: string;
  tables!: any[];
  editedNames!: any[];

  constructor(
    private headerService: HeaderService,
    private authenticationService: AuthService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const jwtToken = JSON.parse(
      atob(localStorage.getItem(ACCESS_TOKEN_KEY)!.split('.')[1])
    );
    this.userName = jwtToken.login;
    this.editedNames = [];
  }

  ngAfterViewInit() {
    this.tables = JSON.parse(localStorage.getItem('tables')!);
    this.tables.forEach((table) => {
      this.editedNames.push(table.name.split(/(?=[A-Z])/).join(' '));
    });
    this.cd.detectChanges();
  }

  selectTable(selectedTable: any) {
    this.headerService.selectTable(selectedTable);
  }

  logOut() {
    this.authenticationService.logout();
  }
}
