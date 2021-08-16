import { Validators } from '@angular/forms';
import { AbstractControl } from '@angular/forms';

export class InputValidator {
  static isSpacesValid: { [id: string]: boolean } = {};
  static isNumbersValid: { [id: string]: boolean } = {};

  static spacesValidator(AC: AbstractControl) {
    if (AC && AC.value && AC.value.trim() === '') {
      return { spacesValidator: true };
    }
    return Validators.nullValidator;
  }

  static numbersValidator(AC: AbstractControl) {
    if (AC && AC.value && /^\d+$/.test(AC.value.replace(/\s+/g, ''))) {
      return { numbersValidator: true };
    }
    return Validators.nullValidator;
  }
}
