import {
  Component,
  AfterViewInit,
  ElementRef,
  ViewChild,
  EventEmitter,
  forwardRef,
  OnInit,
  Input,
} from '@angular/core';

import { DefaultEditor } from 'ng2-smart-table';

import {
  ControlValueAccessor,
  Validators,
  NG_VALUE_ACCESSOR,
  FormControl,
} from '@angular/forms';

import { InputValidator } from '../validators/input-validator';
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
export class InputComponent
  extends DefaultEditor
  implements OnInit, AfterViewInit, ControlValueAccessor
{
  //Get data from hidden div => in ng2-smart-table documentation
  @ViewChild('htmlValue') htmlValue!: ElementRef;
  //Validation
  @Input() formControl!: FormControl;
  static onAdd: EventEmitter<any> = new EventEmitter<any>();
  matcher: MyErrorStateMatcher = new MyErrorStateMatcher(false);
  isAdded: boolean = false;

  onChangeCallback = (_: any) => {
    this.cell.newValue = _;
  };
  onTouchCallback = () => {};

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
        this.formControl.setValue(this.formControl.value.trim());
      }

      if (this.formControl.valid) {
        this.onChangeCallback(this.formControl.value);
      } else {
        this.onChangeCallback('');
      }
    });
  }

  ngAfterViewInit(): void {
    if (this.cell.newValue !== '') {
      this.writeValue(this.getValue());
    }
    InputComponent.onAdd.subscribe(() => {
      if (this.formControl.invalid) {
        this.matcher = new MyErrorStateMatcher(true);
        this.isAdded = true;
      }
    });
    this.onEdited.subscribe(() => {
      if (this.formControl.invalid) {
        this.matcher = new MyErrorStateMatcher(true);
        this.isAdded = true;
      }
    });
  }

  writeValue(value: string): void {
    this.formControl.setValue(value);
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

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
