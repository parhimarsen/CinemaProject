import {
  AbstractControl,
  FormArray,
  ValidationErrors,
  Validators,
} from '@angular/forms';

export class ConfirmedValidator {
  static confirmedValidator(
    controlIndex: number,
    matchingControlIndex: number
  ) {
    return (formGroup: AbstractControl): ValidationErrors | null => {
      const control = (formGroup.get('registerControls') as FormArray).controls[
        controlIndex
      ];
      const matchingControl = (formGroup.get('registerControls') as FormArray)
        .controls[matchingControlIndex];
      if (
        matchingControl.errors &&
        !matchingControl.errors.confirmedValidator
      ) {
        return Validators.nullValidator;
      }
      if (control.value !== matchingControl.value) {
        return { confirmedValidator: true };
      } else {
        return Validators.nullValidator;
      }
    };
  }
}
