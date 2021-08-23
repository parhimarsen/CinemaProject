import { ErrorStateMatcher } from '@angular/material/core';

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isValid!: boolean;

  constructor(isValid: boolean) {
    this.isValid = isValid;
  }

  isErrorState(): boolean {
    return this.isValid;
  }
}
