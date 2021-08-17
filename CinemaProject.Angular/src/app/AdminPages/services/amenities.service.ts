import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Ng2InputComponent } from '../MainPage/custom/input/ng2-input.component';
import { CurrencyInputComponent } from '../MainPage/custom/currency-input/currency-input.component';

import { Amenity, AmenityView } from '../Models/amenity';
import { InputValidator } from '../MainPage/custom/validators/input-validator';

@Injectable({
  providedIn: 'root',
})
export class AmenitiesService {
  url = 'https://localhost:44356/api/Amenities';

  source: LocalDataSource;
  settings: any;

  isComplited = new BehaviorSubject<boolean>(false);

  amenities: Amenity[];
  amenitiesView: AmenityView[];

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {
    this.amenities = [];
    this.amenitiesView = [];
    this.source = new LocalDataSource();
  }

  private handleError<Amenity>(operation = 'operation', result?: Amenity) {
    return (error: any): Observable<Amenity> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as Amenity);
    };
  }

  getAll(): Observable<Amenity[]> {
    return this.http
      .get<Amenity[]>(this.url)
      .pipe(catchError(this.handleError<Amenity[]>('getAmenities', [])));
  }

  private postRequest(amenity: Amenity): Observable<Amenity> {
    return this.http
      .post<Amenity>(this.url, amenity, this.httpOptions)
      .pipe(catchError(this.handleError<Amenity>('addAmenity')));
  }

  private deleteRequest(id: string): Observable<Amenity> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<Amenity>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Amenity>('deleteAmenity')));
  }

  private putRequest(amenity: Amenity): Observable<any> {
    return this.http
      .put(this.url, amenity, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateAmenity')));
  }

  private convertAmenitiesToView(amenities: Amenity[]): AmenityView[] {
    return amenities
      .map((amenity) => {
        return new AmenityView(
          amenity.id,
          amenity.name,
          new Intl.NumberFormat('fr-BR', {
            style: 'currency',
            currency: 'BYN',
          }).format(parseFloat(amenity.cost.toFixed(2))),
          amenity.extraPaymentPercent
        );
      })
      .sort((a, b) => {
        if (a.name > b.name) {
          return 1;
        }
        if (a.name < b.name) {
          return -1;
        }
        return 0;
      });
  }

  refreshData() {
    this.getAll().subscribe((amenities) => {
      this.amenities = amenities;
      this.amenitiesView = this.convertAmenitiesToView(this.amenities);

      this.settings = {
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
          name: {
            title: 'Name',
            filter: false,
            editor: {
              type: 'custom',
              component: Ng2InputComponent,
            },
          },
          cost: {
            title: 'Cost',
            filter: false,
            editor: {
              type: 'custom',
              component: CurrencyInputComponent,
            },
          },
        },
      };
      this.source = new LocalDataSource(this.amenitiesView);
      this.isComplited.next(true);
    });
  }

  onSearch(query: string) {
    if (query === '') {
      this.source.setFilter([]);
    } else {
      this.source.setFilter(
        [
          {
            field: 'name',
            search: query,
          },
          {
            field: 'cost',
            search: query,
          },
        ],
        false
      );
    }
  }

  add(event: any): void {
    let amenity = event.newData;
    let isValid = true;

    for (let field in InputValidator.isNumbersValid) {
      if (!InputValidator.isNumbersValid[field]) return;
    }
    for (let field in InputValidator.isSpacesValid) {
      if (!InputValidator.isSpacesValid[field]) return;
    }

    for (let key in amenity) {
      if (amenity[key] === '') {
        isValid = false;
        break;
      }
      amenity[key] = amenity[key].trim();
    }

    if (!isValid) return;

    let newAmenity = {
      name: amenity.name,
      cost: amenity.cost
        .replace(/\s/g, '')
        .replace('BYN', '')
        .replace(',', '.'),
    };
    if (!newAmenity) return;
    this.postRequest(newAmenity as Amenity).subscribe(() => {
      this.refreshData();
    });
  }

  delete(event: any): void {
    let amenity = event.data;

    this.deleteRequest(amenity.id).subscribe(() => {
      this.refreshData();
    });
  }

  edit(event: any): void {
    let amenity = event.newData;
    let isValid = true;

    for (let field in InputValidator.isNumbersValid) {
      if (!InputValidator.isNumbersValid[field]) return;
    }
    for (let field in InputValidator.isSpacesValid) {
      if (!InputValidator.isSpacesValid[field]) return;
    }

    for (let key in amenity) {
      if (amenity[key] === '') {
        isValid = false;
        break;
      }
      amenity[key] = amenity[key].trim();
    }

    if (!isValid) return;

    amenity = new Amenity(
      amenity.id,
      amenity.name,
      amenity.cost.replace(/\s/g, '').replace('BYN', '').replace(',', '.'),
      0
    );
    this.putRequest(amenity).subscribe(() => {
      this.refreshData();
    });
  }
}
