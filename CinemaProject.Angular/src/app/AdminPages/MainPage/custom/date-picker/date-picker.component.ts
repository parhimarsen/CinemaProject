import {
  Component,
  AfterViewInit,
  Input,
  ViewChild,
  ElementRef,
  ChangeDetectorRef,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DefaultEditor } from 'ng2-smart-table';
import * as moment from 'moment';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.css'],
})
export class DatePickerComponent
  extends DefaultEditor
  implements AfterViewInit
{
  @ViewChild('inputValue') inputValue!: ElementRef;
  @ViewChild('htmlValue') htmlValue!: ElementRef;
  @Input() releaseDate!: Date;
  @Input() formGroup: FormGroup;

  constructor(private cdr: ChangeDetectorRef) {
    super();
    this.formGroup = new FormGroup({
      value: new FormControl(
        { value: '', disabled: false },
        Validators.required
      ),
    });
    this.formGroup.markAllAsTouched();
  }

  ngAfterViewInit(): void {
    if (this.cell.newValue !== '') {
      this.releaseDate = new Date(
        this.getValue().split('-').reverse().join('-')
      );
    }
    this.cdr.detectChanges();
  }

  updateValue() {
    if (this.releaseDate != undefined) {
      this.cell.newValue =
        moment(this.releaseDate).format().toString().split('+')[0] + '.000Z';
    }
  }

  validate(event: any) {
    if (
      this.inputValue.nativeElement.value &&
      this.inputValue.nativeElement.value.length > 10
    ) {
      this.inputValue.nativeElement.value =
        this.inputValue.nativeElement.value.slice(0, 10);
    }

    let theEvent = event || window.event;
    let key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    let regex = /[0-9]|\//;
    if (!regex.test(key)) {
      theEvent.returnValue = false;
      if (theEvent.preventDefault) theEvent.preventDefault();
    }
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
