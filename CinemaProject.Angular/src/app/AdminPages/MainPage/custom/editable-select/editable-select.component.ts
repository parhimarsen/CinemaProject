import {
  Component,
  Input,
  AfterViewInit,
  ChangeDetectorRef,
} from '@angular/core';

import { HttpHeaders } from '@angular/common/http';

import { DefaultEditor } from 'ng2-smart-table';
import { ModalComponent } from '../modal/modal.component';

import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { CinemasService } from 'src/app/AdminPages/services/cinemas.service';
import { HallsService } from 'src/app/AdminPages/services/halls.service';
import { SeatsService } from 'src/app/AdminPages/services/seats.service';
import { TypesOfSeatService } from 'src/app/AdminPages/services/types-of-seat.service';

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
  selectedValue: any;
  modalRef!: BsModalRef;
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
    private seatsService: SeatsService
  ) {
    super();
  }

  ngAfterViewInit(): void {
    this.value.sort((a, b) => {
      if (a.name > b.name) {
        return 1;
      }
      if (a.name < b.name) {
        return -1;
      }
      return 0;
    });
    this.cdr.detectChanges();
  }

  changeTypeOfSeat(typeOfSeat: any): void {
    this.modalHeaderText = 'Edit type of seat';
    this.typeOfEvent = 'Edit';

    const initialState = {
      value: this.value,
      editedTypeOfSeat: typeOfSeat,
      modalHeaderText: this.modalHeaderText,
      typeOfEvent: this.typeOfEvent,
    };

    this.modalRef = this.modalService.show(ModalComponent, { initialState });
  }

  openAddModal(): void {
    this.modalHeaderText = 'Add new type of seat';
    this.typeOfEvent = 'Add';

    const initialState = {
      value: this.value,
      modalHeaderText: this.modalHeaderText,
      typeOfEvent: this.typeOfEvent,
    };

    this.modalRef = this.modalService.show(ModalComponent, { initialState });
  }

  delete(typeOfSeat: any): void {
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
