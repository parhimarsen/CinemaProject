import {
  Component,
  Input,
  AfterViewInit,
  ChangeDetectorRef,
} from '@angular/core';

import { HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

import { DefaultEditor } from 'ng2-smart-table';
import { ModalComponent } from '../modal/modal.component';

import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { CinemasService } from 'src/app/AdminPages/services/cinemas.service';
import { HallsService } from 'src/app/AdminPages/services/halls.service';
import { SeatsService } from 'src/app/AdminPages/services/seats.service';
import { TypesOfSeatService } from 'src/app/AdminPages/services/types-of-seat.service';
import { TypeOfSeat } from 'src/app/AdminPages/Models/typeOfSeat';
import { AmenitiesService } from 'src/app/AdminPages/services/amenities.service';
import { AmenityView } from 'src/app/AdminPages/Models/amenity';

@Component({
  selector: 'app-editable-select',
  templateUrl: './editable-select.component.html',
  styleUrls: ['./editable-select.component.css'],
})
export class EditableSelectComponent
  extends DefaultEditor
  implements AfterViewInit
{
  @Input() value!: any[];
  modalRef!: BsModalRef;
  url: string[];
  modalHeaderText = '';
  typeOfEvent = '';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(
    private cdr: ChangeDetectorRef,
    private modalService: BsModalService,
    private typesOfSeatService: TypesOfSeatService,
    private cinemasService: CinemasService,
    private hallsService: HallsService,
    private seatsService: SeatsService,
    private amenitiesService: AmenitiesService,
    private router: Router
  ) {
    super();
    this.url = this.router.url.split('/');
  }

  ngAfterViewInit(): void {
    this.cdr.detectChanges();
  }

  parseFloat(str: string): number {
    str = str.replace(',', '.');
    return Number.parseFloat(str);
  }

  changeTypeOfSeat(typeOfSeat: TypeOfSeat): void {
    this.modalHeaderText = 'Edit type of seat';
    this.typeOfEvent = 'Edit TypeOfSeat';

    const initialState = {
      value: this.value,
      editedTypeOfSeat: typeOfSeat,
      modalHeaderText: this.modalHeaderText,
      typeOfEvent: this.typeOfEvent,
    };

    this.modalRef = this.modalService.show(ModalComponent, { initialState });
  }

  addTypeOfSeat(): void {
    this.modalHeaderText = 'Add new type of seat';
    this.typeOfEvent = 'Add TypeOfSeat';

    const initialState = {
      value: this.value,
      modalHeaderText: this.modalHeaderText,
      typeOfEvent: this.typeOfEvent,
    };

    this.modalRef = this.modalService.show(ModalComponent, { initialState });
  }

  editAmenities(): void {
    this.modalHeaderText = 'Edit affortable services of session';
    this.typeOfEvent = 'Edit Amenities';

    this.amenitiesService.getAll().subscribe((amenities) => {
      const initialState = {
        value: amenities.map((amenity) => {
          return new AmenityView(
            amenity.id,
            amenity.name,
            amenity.cost.toFixed(2),
            amenity.extraPaymentPercent,
            this.value[0].id
          );
        }),
        selectedAmenities: this.value[0].affortableAmenities,
        modalHeaderText: this.modalHeaderText,
        typeOfEvent: this.typeOfEvent,
      };

      this.modalRef = this.modalService.show(ModalComponent, { initialState });
    });
  }

  deleteTypeOfSeat(typeOfSeat: TypeOfSeat): void {
    if (typeOfSeat.name !== 'Common') {
      this.typesOfSeatService.deleteRequest(typeOfSeat.id).subscribe(() => {
        this.value.splice(this.value.indexOf(typeOfSeat), 1);
        this.cdr.detectChanges();
      });
    }

    // All Seats of deleted TypeOfSeat update to Common
    this.cinemasService.getAll().subscribe((cinemas) => {
      let cinema = cinemas.find((cinema) => cinema.id === typeOfSeat.cinemaId);

      let commonTypeOfSeat = cinema?.typesOfSeat!.find(
        (typeOfSeat) =>
          typeOfSeat.cinemaId === cinema?.id && typeOfSeat.name === 'Common'
      );

      cinema?.halls?.forEach((hall) => {
        this.hallsService.getSeats(hall.id).subscribe((seats) => {
          seats.forEach((seat) => {
            if (seat.typeOfSeatId === null) {
              seat.typeOfSeatId = commonTypeOfSeat!.id;
              this.seatsService.putSeatRequest(seat).subscribe();
            }
          });
        });
      });
    });
  }
}
