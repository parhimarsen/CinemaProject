import { Component, Input, OnInit } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { HeaderService } from '../../services/header.service';

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

  constructor(private headerService: HeaderService) {}

  ngOnInit(): void {
    this.headerService.selectTable({
      serviceName: this.service.constructor.name.substring(
        0,
        this.service.constructor.name.length - 7
      ),
    });
    //Update data source of service
    this.service.refreshData();
    //Get data source and settings from service
    this.service.isComplited.subscribe(() => {
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
