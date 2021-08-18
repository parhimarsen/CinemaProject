import { Component, forwardRef, Input, EventEmitter } from '@angular/core';

import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  FormControl,
} from '@angular/forms';

import { MyErrorStateMatcher } from '../validators/my-error-state-matcher';

@Component({
  selector: 'app-label',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true,
    },
  ],
})
export class InputComponent implements ControlValueAccessor {
  @Input() formControl: FormControl = new FormControl();
  @Input() cell: any;
  @Input() matcher: MyErrorStateMatcher = new MyErrorStateMatcher(false);
  @Input() onEdited!: EventEmitter<any>;
  isAdded: boolean = false;

  constructor() {}

  onChangeCallback = (_: any) => {
    this.cell.newValue = _;
  };

  onTouchCallback = () => {};

  writeValue(value: any): void {
    value
      ? this.formControl.setValue(value)
      : this.formControl.setValue(this.cell.getValue());
  }

  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchCallback = fn;
  }

  onKeyPress(event: any): void {
    var theEvent = event || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[a-zA-Z]|[0-9]|[\s]/;
    if (!regex.test(key)) {
      theEvent.returnValue = false;
      if (theEvent.preventDefault) theEvent.preventDefault();
    }
  }
}
