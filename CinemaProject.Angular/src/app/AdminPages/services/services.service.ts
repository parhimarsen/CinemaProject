import { Injectable } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';

import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { InputComponent } from '../MainPage/custom/input/input.component';
import { CurrencyInputComponent } from '../MainPage/custom/currency-input/currency-input.component';

import { Service, ServiceView } from '../Models/service';
import { InputValidator } from '../MainPage/custom/validators/input-validator';

@Injectable({
  providedIn: 'root',
})
export class ServicesService {
  url = 'https://localhost:44356/api/Services';

  source: LocalDataSource;
  settings: any;

  isComplited = new BehaviorSubject<boolean>(false);

  services: Service[];
  servicesView: ServiceView[];

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {
    this.services = [];
    this.servicesView = [];
    this.source = new LocalDataSource();
  }

  private handleError<Service>(operation = 'operation', result?: Service) {
    return (error: any): Observable<Service> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as Service);
    };
  }

  private getAll(): Observable<Service[]> {
    return this.http
      .get<Service[]>(this.url)
      .pipe(catchError(this.handleError<Service[]>('getServices', [])));
  }

  private postRequest(service: Service): Observable<Service> {
    return this.http
      .post<Service>(this.url, service, this.httpOptions)
      .pipe(catchError(this.handleError<Service>('addService')));
  }

  private deleteRequest(id: string): Observable<Service> {
    const url = `${this.url}/${id}`;

    return this.http
      .delete<Service>(url, this.httpOptions)
      .pipe(catchError(this.handleError<Service>('deleteService')));
  }

  private putRequest(service: Service): Observable<any> {
    return this.http
      .put(this.url, service, this.httpOptions)
      .pipe(catchError(this.handleError<any>('updateService')));
  }

  private convertServicesToView(services: Service[]): ServiceView[] {
    return services
      .map((service) => {
        return new ServiceView(
          service.id,
          service.name,
          new Intl.NumberFormat('fr-BR', {
            style: 'currency',
            currency: 'BYN',
          }).format(parseFloat(service.cost.toFixed(2)))
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
    this.getAll().subscribe((services) => {
      console.log(services);
      this.services = services;
      this.servicesView = this.convertServicesToView(this.services);

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
              component: InputComponent,
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
      this.source = new LocalDataSource(this.servicesView);
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
    let service = event.newData;
    let isValid = true;

    for (let field in InputValidator.isNumbersValid) {
      if (!InputValidator.isNumbersValid[field]) return;
    }
    for (let field in InputValidator.isSpacesValid) {
      if (!InputValidator.isSpacesValid[field]) return;
    }

    for (let key in service) {
      if (service[key] === '') {
        isValid = false;
        break;
      }
      service[key] = service[key].trim();
    }

    if (!isValid) return;

    let newService = {
      name: service.name,
      cost: service.cost
        .replace(/\s/g, '')
        .replace('BYN', '')
        .replace(',', '.'),
    };
    if (!newService) return;
    this.postRequest(newService as Service).subscribe(() => {
      this.refreshData();
    });
  }

  delete(event: any): void {
    let service = event.data;

    this.deleteRequest(service.id).subscribe(() => {
      this.refreshData();
    });
  }

  edit(event: any): void {
    let service = event.newData;
    let isValid = true;

    for (let field in InputValidator.isNumbersValid) {
      if (!InputValidator.isNumbersValid[field]) return;
    }
    for (let field in InputValidator.isSpacesValid) {
      if (!InputValidator.isSpacesValid[field]) return;
    }

    for (let key in service) {
      if (service[key] === '') {
        isValid = false;
        break;
      }
      service[key] = service[key].trim();
    }

    if (!isValid) return;

    service = new Service(
      service.id,
      service.name,
      service.cost.replace(/\s/g, '').replace('BYN', '').replace(',', '.')
    );
    this.putRequest(service).subscribe(() => {
      this.refreshData();
    });
  }
}
