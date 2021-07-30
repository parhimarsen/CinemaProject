import { Component, Input, OnInit } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
//ng2-smart-table
export class TableComponent implements OnInit {
  //Dynamic service depending on routing
  @Input() service: any;

  //Properties of table
  settings = {};
  //Table data source
  source = new LocalDataSource();

  constructor() {}

  ngOnInit(): void {
    //Update data source of service
    this.service.refreshData();
    //Get data source and settings from service
    this.service.data.subscribe(() => {
      this.source = this.service.source;
      this.settings = Object.assign({}, this.service.settings);
    });
  }

  onSearch(query: string = '') {
    this.service.onSearch(query);
  }

  add(event: any): void {
    this.service.add(event);
  }

  delete(event: any): void {
    this.service.delete(event);
  }

  edit(event: any): void {
    this.service.edit(event);
  }
}
