import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HeaderService {
  tables = [
    { name: 'Cinemas', serviceName: 'Cinemas', isActive: true },
    { name: 'Halls', serviceName: 'Halls', isActive: false },
    { name: 'Films', serviceName: 'Films', isActive: false },
    { name: 'Sessions', serviceName: 'Sessions', isActive: false },
    { name: 'Services', serviceName: 'Amenities', isActive: false },
    { name: 'Cities', serviceName: 'Cities', isActive: false },
  ];
  userName = 'parhimarsen@mail.ru';

  constructor() {}

  selectTable(selectedTable: any) {
    this.tables.forEach((table) => {
      table.isActive = false;
    });
    this.tables.find(
      (table) => table.serviceName === selectedTable.serviceName
    )!.isActive = true;
    localStorage.setItem('tables', JSON.stringify(this.tables));
  }
}
