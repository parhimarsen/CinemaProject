import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HeaderService {
  tables = [
    { name: 'Cinemas', isActive: true },
    { name: 'Halls', isActive: false },
    { name: 'Films', isActive: false },
    { name: 'Sessions', isActive: false },
    { name: 'Services', isActive: false },
    { name: 'TypesOfSeat', isActive: false },
  ];
  userName = 'parhimarsen@mail.ru';

  constructor() {}

  selectTable(selectedTable: any) {
    this.tables.forEach((table) => {
      table.isActive = false;
    });
    this.tables.find((table) => table.name === selectedTable.name)!.isActive =
      true;
    localStorage.setItem('tables', JSON.stringify(this.tables));
  }
}
