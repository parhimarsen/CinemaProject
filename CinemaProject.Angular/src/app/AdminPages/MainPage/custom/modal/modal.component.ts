import { ThrowStmt } from '@angular/compiler';
import {
  Component,
  AfterViewInit,
  ChangeDetectorRef,
  ElementRef,
  ViewChildren,
  QueryList,
} from '@angular/core';

import { FormControl, FormGroup, Validators } from '@angular/forms';

import { DefaultEditor } from 'ng2-smart-table';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Amenity, AmenityView } from 'src/app/AdminPages/Models/amenity';

import { CinemasService } from 'src/app/AdminPages/services/cinemas.service';
import { SessionsService } from 'src/app/AdminPages/services/sessions.service';
import { TypesOfSeatService } from 'src/app/AdminPages/services/types-of-seat.service';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
})
export class ModalComponent extends DefaultEditor implements AfterViewInit {
  @ViewChildren('inputValue') inputValues!: QueryList<ElementRef>;
  value!: any[];
  editedTypeOfSeat!: any;
  selectedAmenities!: any[];

  name!: string;
  placeholderName = 'Name';
  extraPaymentPercent!: string;
  placeholderExtraPaymentPercent = 'ExtraPaymentPercent';
  checked = false;

  modalHeaderText!: string;
  typeOfEvent!: string;
  formGroup: FormGroup;

  constructor(
    public modalRef: BsModalRef,
    private cdr: ChangeDetectorRef,
    private typesOfSeatService: TypesOfSeatService,
    private cinemasService: CinemasService,
    private sessionsService: SessionsService
  ) {
    super();

    this.formGroup = new FormGroup({
      name: new FormControl({ name: '', disabled: false }, Validators.required),
      extraPaymentPercent: new FormControl(
        { extraPaymentPercent: '', disabled: false },
        [Validators.required, Validators.min(-100), Validators.max(1000)]
      ),
    });

    this.formGroup.markAllAsTouched();
  }

  ngAfterViewInit(): void {
    if (this.typeOfEvent === 'Edit TypeOfSeat') {
      this.name = this.editedTypeOfSeat.name;
      this.extraPaymentPercent = this.editedTypeOfSeat.extraPaymentPercent;
    }
    if (this.typeOfEvent === 'Edit Amenities') {
      this.selectedAmenities.forEach((selectedAmenity) => {
        let amenity = this.value.find((v) => v.id === selectedAmenity.id);
        console.log(selectedAmenity);
        amenity.extraPaymentPercent = selectedAmenity.extraPaymentPercent;
        amenity.isChecked = true;
      });
    }
    this.cdr.detectChanges();
  }

  parseFloat(str: string): number {
    return Number.parseFloat(str);
  }

  maxLength(): void {
    if (
      this.extraPaymentPercent &&
      this.extraPaymentPercent.toString().length >= 5
    ) {
      this.extraPaymentPercent = this.extraPaymentPercent
        .toString()
        .slice(0, 5);
    }
  }

  maxLengthForAmenities(amenity: AmenityView): void {
    let inputValue = this.inputValues.get(this.value.indexOf(amenity))!;
    if (inputValue.nativeElement.value.length >= 6) {
      inputValue.nativeElement.value = amenity.extraPaymentPercent
        .toString()
        .slice(0, 6);
      amenity.extraPaymentPercent = parseFloat(
        amenity.extraPaymentPercent.toString().slice(0, 6)
      );
    }
  }

  setZero(amenity: AmenityView): void {
    if (
      amenity.extraPaymentPercent == null ||
      amenity.extraPaymentPercent === 0
    ) {
      amenity.extraPaymentPercent = 0;
    }
  }

  addTypeOfSeat(): void {
    let newTypeOfSeat = this.formGroup.value;

    newTypeOfSeat = {
      cinemaId: this.value[0].cinemaId,
      name: newTypeOfSeat.name,
      extraPaymentPercent: newTypeOfSeat.extraPaymentPercent,
    };

    this.typesOfSeatService
      .postRequest(newTypeOfSeat)
      .subscribe((typeOfSeat) => {
        this.name = '';
        this.extraPaymentPercent = '';
        this.value.push(typeOfSeat);
        this.modalRef.hide();
      });
  }

  editTypeOfSeat(): void {
    let newTypeOfSeat = this.editedTypeOfSeat;

    this.cinemasService.getAll().subscribe((cinemas) => {
      let cinema = cinemas.find(
        (cinema) => cinema.id === newTypeOfSeat.cinemaId
      );

      newTypeOfSeat = {
        id: newTypeOfSeat.id,
        cinemaId: cinema!.id,
        name: this.formGroup.value.name,
        extraPaymentPercent: this.formGroup.value.extraPaymentPercent,
      };

      this.typesOfSeatService.putRequest(newTypeOfSeat).subscribe(() => {
        this.value.splice(this.value.indexOf(newTypeOfSeat), 1);
        this.value.push(newTypeOfSeat);
        this.modalRef.hide();
        this.cdr.detectChanges();
      });
    });
  }

  checkAmenity(amenityView: AmenityView, event: any): void {
    if (event.checked) {
      let amenity: Amenity = new Amenity(
        amenityView.id,
        amenityView.name,
        parseFloat(amenityView.cost),
        amenityView.extraPaymentPercent
      );
      this.sessionsService
        .postAmenity(amenityView.sessionId!, amenity)
        .subscribe(() => {
          this.selectedAmenities.push(amenityView);
        });
    } else {
      this.sessionsService
        .deleteAmenity(amenityView.sessionId!, amenityView.id)
        .subscribe(() => {
          this.selectedAmenities.splice(
            this.selectedAmenities.findIndex((v) => v.id === amenityView.id),
            1
          );

          let amenity = this.value.find((v) => v.id === amenityView.id);
          amenity.isChecked = false;
          console.log(this.value);
          this.cdr.detectChanges();
        });
    }
  }
}
