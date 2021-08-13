import { Validators } from '@angular/forms';
import { AbstractControl } from '@angular/forms';

export class DateTimeValidator {
  static isDateValid: { [id: string]: boolean } = {};

  static pastDatesValidator(AC: AbstractControl) {
    if (AC && AC.value && new Date() >= AC.value) {
      return { pastDatesValidator: true };
    }
    return Validators.nullValidator;
  }
}
