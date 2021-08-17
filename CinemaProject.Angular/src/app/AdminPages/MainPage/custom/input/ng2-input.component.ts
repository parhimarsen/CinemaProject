import { Component, AfterViewInit, EventEmitter, OnInit } from '@angular/core';

import { DefaultEditor } from 'ng2-smart-table';

import { Validators, FormControl } from '@angular/forms';

import { InputValidator } from '../validators/input-validator';
import { MyErrorStateMatcher } from '../validators/my-error-state-matcher';

@Component({
  selector: 'ng2-input',
  template: `<app-label
    [formControl]="formControl"
    [cell]="cell"
    [matcher]="matcher"
  ></app-label>`,
  styles: [],
})
export class Ng2InputComponent
  extends DefaultEditor
  implements OnInit, AfterViewInit
{
  formControl!: FormControl;
  static onAdd: EventEmitter<any> = new EventEmitter<any>();
  matcher: MyErrorStateMatcher = new MyErrorStateMatcher(false);
  isAdded: boolean = false;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.formControl = new FormControl('', {
      validators: [
        Validators.required,
        InputValidator.spacesValidator,
        InputValidator.numbersValidator,
      ],
    });
    this.formControl.valueChanges.subscribe(() => {
      if (this.isAdded) {
        this.matcher = new MyErrorStateMatcher(false);
        this.isAdded = false;
      }

      if (this.formControl.valid) {
        this.cell.newValue = this.formControl.value;
      }
    });
  }

  ngAfterViewInit(): void {
    if (this.cell.newValue !== '') {
      this.cell.newValue = this.formControl.value;
    }
    Ng2InputComponent.onAdd.subscribe(() => {
      if (this.formControl.invalid) {
        this.matcher = new MyErrorStateMatcher(true);
        this.isAdded = true;
      }
    });
  }
}
