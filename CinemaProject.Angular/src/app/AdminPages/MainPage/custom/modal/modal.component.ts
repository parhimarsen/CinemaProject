import { Component, AfterViewInit, ChangeDetectorRef } from '@angular/core';

import { FormControl, FormGroup, Validators } from '@angular/forms';

import { DefaultEditor } from 'ng2-smart-table';
import { BsModalRef } from 'ngx-bootstrap/modal';

import { CinemasService } from 'src/app/AdminPages/services/cinemas.service';
import { TypesOfSeatService } from 'src/app/AdminPages/services/types-of-seat.service';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
})
export class ModalComponent extends DefaultEditor implements AfterViewInit {
  value!: any[];
  editedTypeOfSeat!: any;

  name!: string;
  placeholderName = 'Name';
  extraPaymentPercent!: string;
  placeholderExtraPaymentPercent = 'ExtraPaymentPercent';

  modalHeaderText!: string;
  typeOfEvent!: string;
  formGroup: FormGroup;

  constructor(
    public modalRef: BsModalRef,
    private cdr: ChangeDetectorRef,
    private typesOfSeatService: TypesOfSeatService,
    private cinemasService: CinemasService
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

  maxLength(): void {
    if (
      this.extraPaymentPercent &&
      this.extraPaymentPercent.toString().length >= 6
    ) {
      this.extraPaymentPercent = this.extraPaymentPercent
        .toString()
        .slice(0, 6);
    }
  }

  ngAfterViewInit(): void {
    if (this.typeOfEvent === 'Edit') {
      this.name = this.editedTypeOfSeat.name;
      this.extraPaymentPercent = this.editedTypeOfSeat.extraPaymentPercent;
    }
    this.cdr.detectChanges();
  }

  add(): void {
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
      });
  }

  edit(): void {
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
}
