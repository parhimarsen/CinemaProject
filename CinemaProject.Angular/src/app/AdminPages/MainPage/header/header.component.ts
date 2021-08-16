import { Component, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { HeaderService } from '../../services/header.service';

@Component({
  selector: 'app-admin-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements AfterViewInit {
  userName!: string;
  tables!: any[];
  editedNames: any[];

  constructor(
    private headerService: HeaderService,
    private cd: ChangeDetectorRef
  ) {
    this.userName = headerService.userName;
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
}
