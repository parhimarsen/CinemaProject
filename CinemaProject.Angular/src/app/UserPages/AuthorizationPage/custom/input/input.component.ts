import {
  Component,
  EventEmitter,
  forwardRef,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MyFormControl } from '../../auth-form/auth-form.component';
import { MyErrorStateMatcher } from '../validators/error-state-matcher';

@Component({
  selector: 'custom-input',
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
export class InputComponent implements OnInit, ControlValueAccessor {
  @Input() control: MyFormControl = new MyFormControl();
  @Input() placeholder!: string;
  @Input() type!: string;
  @Input() matcher!: MyErrorStateMatcher;
  @Output() onPasswordChange: EventEmitter<any> = new EventEmitter<any>();

  private onChangeCallback = (_: any) => {
    if (this.control.isSubmitted) {
      if (this.control.placeholder.toLowerCase().includes('password')) {
        this.onPasswordChange.emit();
      }
      this.control.isSubmitted = false;
      this.matcher.isValid = false;
    }
  };
  private onTouchCallback = (_: any) => {};

  constructor() {}

  ngOnInit(): void {
    this.control.valueChanges.subscribe((value) => {
      this.onChangeCallback(value);
    });
  }

  writeValue(obj: any): void {
    if (obj) {
      this.control.setValue(obj);
    }
  }

  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchCallback = fn;
  }
}
