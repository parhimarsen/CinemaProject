import { Component, OnInit, OnChanges } from '@angular/core';
import { StyleService } from '../main/style.service';
import { LocalDataSource } from 'ng2-smart-table';

import { Cinema, CinemaView } from '../../Models/cinema';
import { CinemasService } from '../../cinemas.service';
import { City } from '../../Models/city';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
export class TableComponent implements OnInit {
  source: LocalDataSource;
  cinemas: Cinema[];
  cities: City[] = [];
  cinemasView: CinemaView[];
  isDisplayNone: boolean = true;

  settings = {};

  constructor(
    public styleService: StyleService,
    private cinemasService: CinemasService
  ) {
    this.cinemas = [];
    this.cinemasView = [];
    this.source = new LocalDataSource(this.cinemasView);
  }

  ngOnInit(): void {
    this.getCinemas();
    this.getCities();
  }

  onSearch(query: string = '') {
    if (query === '') {
      this.source.setFilter([]);
    } else {
      this.source.setFilter(
        [
          {
            field: 'id',
            search: query,
          },
          {
            field: 'name',
            search: query,
          },
          {
            field: 'cityName',
            search: query,
          },
        ],
        false
      );
    }
  }

  getCinemas(): void {
    this.cinemasService.getCinemas().subscribe((cinemas) => {
      this.cinemas = cinemas;
      cinemas.forEach((cinema, index) => {
        this.cinemasService.getCityOfCinema(cinema.id).subscribe((city) => {
          let cinemaView = {} as CinemaView;
          cinemaView.id = index + 1;
          cinemaView.name = cinema.name;
          cinemaView.cityName = city.name;

          this.cinemasView.sort((a, b) => a.id - b.id);
          this.source.refresh();
        });
      });
    });
  }

  getCities(): void {
    this.cinemasService.getCitites().subscribe((cities) => {
      this.cities = cities;
      let mySettings = {
        add: {
          confirmCreate: true,
        },
        delete: {
          confirmDelete: true,
        },
        edit: {
          confirmSave: true,
        },
        columns: {
          id: {
            title: 'ID',
            filter: false,
            addable: false,
            editable: false,
          },
          name: {
            title: 'Name',
            filter: false,
          },
          cityName: {
            title: 'City',
            filter: false,
            editor: {
              type: 'list',
              config: {
                list: this.cities.map((city) => {
                  return { value: city.name, title: city.name };
                }),
              },
            },
          },
        },
      };
      this.settings = Object.assign({}, mySettings);
    });
  }

  addCinema(event: any): void {
    let cinema = event.newData;

    for (let key in cinema) {
      if (cinema[key] !== null) cinema[key] = cinema[key].trim();
    }

    let cityName: string = cinema.cityName!;
    let cityId = this.cities.find((city) => city.name === cityName)?.id;

    if (cityId !== undefined) {
      let newCinema = { name: cinema.name, cityId: cityId };
      if (!newCinema) return;
      this.cinemasService.addCinema(newCinema as Cinema).subscribe((cinema) => {
        this.source.add({
          id: this.cinemasView.length,
          name: cinema.name,
          cityName: cityName,
        });
        this.source.refresh();
      });
    }
  }

  deleteCinema(event: any): void {
    let cinema = event.data;

    let cinemaName: string = cinema.name!;
    let cinemaId = this.cinemas.find(
      (cinema) => cinema.name === cinemaName
    )?.id;

    if (cinemaId !== undefined) {
      this.cinemasService.deleteCinema(cinemaId).subscribe();
    }

    this.source.remove(cinema);
  }
}
