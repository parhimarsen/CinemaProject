import {
  Component,
  AfterViewInit,
  ElementRef,
  ViewChild,
  ChangeDetectorRef,
} from '@angular/core';
import { DefaultEditor } from 'ng2-smart-table';

import { FormControl, FormGroup, Validators } from '@angular/forms';
import { InputValidator } from '../validators/input-validator';

@Component({
  selector: 'app-label',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.css'],
})
export class InputComponent extends DefaultEditor implements AfterViewInit {
  //Get data from hidden div => in ng2-smart-table documentation
  @ViewChild('htmlValue') htmlValue!: ElementRef;
  //Value of Input
  value!: string;
  //Validation
  formGroup: FormGroup;

  constructor(private cdr: ChangeDetectorRef) {
    super();
    this.formGroup = new FormGroup({
      value: new FormControl({ value: '', disabled: false }, [
        Validators.required,
        InputValidator.spacesValidator,
        InputValidator.numbersValidator,
      ]),
    });
    this.formGroup.markAllAsTouched();
  }

  ngAfterViewInit(): void {
    if (this.cell.newValue !== '') {
      this.value = this.getValue();
    }
    this.cdr.detectChanges();
  }

  updateValue(event: any): void {
    if (this.formGroup.controls['value'].hasError('spacesValidator')) {
      InputValidator.isSpacesValid[`${this.cell.getTitle()}`] = false;
    } else {
      InputValidator.isSpacesValid[`${this.cell.getTitle()}`] = true;
    }

    if (this.formGroup.controls['value'].hasError('numbersValidator')) {
      InputValidator.isNumbersValid[`${this.cell.getTitle()}`] = false;
    } else {
      InputValidator.isNumbersValid[`${this.cell.getTitle()}`] = true;
    }

    if (this.value && event.code !== 'Space') {
      this.value = this.value.replace(/\s+/g, ' ');
    }

    this.cell.newValue = this.value;
  }

  validate(event: any): void {
    var theEvent = event || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[a-zA-Z]|[0-9]|[\s]/;
    if (!regex.test(key)) {
      theEvent.returnValue = false;
      if (theEvent.preventDefault) theEvent.preventDefault();
    }
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
