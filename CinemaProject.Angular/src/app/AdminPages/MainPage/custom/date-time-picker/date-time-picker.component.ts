import {
  Component,
  AfterViewInit,
  ViewChild,
  Input,
  ElementRef,
  ChangeDetectorRef,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DefaultEditor } from 'ng2-smart-table';
import * as moment from 'moment';
import { DateTimeValidator } from '../validators/date-time-validator';

@Component({
  selector: 'app-date-time-picker',
  templateUrl: './date-time-picker.component.html',
  styleUrls: ['./date-time-picker.component.css'],
})
export class DateTimePickerComponent extends DefaultEditor implements AfterViewInit
{
  @ViewChild('picker') picker: any;
  @ViewChild('htmlValue') htmlValue!: ElementRef;
  @Input() dateTime!: Date;
  @Input() formGroup: FormGroup;
  labelText: string = 'Choose a date';

  constructor(private cdr: ChangeDetectorRef) {
    super();
    this.formGroup = new FormGroup({
      value: new FormControl({ value: '', disabled: false }, [
        Validators.required,
        DateTimeValidator.pastDatesValidator,
      ]),
    });
    this.formGroup.markAllAsTouched();
  }

  ngAfterViewInit(): void {
    if (this.cell.getTitle() === 'Show End') {
      this.labelText = '';
      this.formGroup.markAsUntouched();
    }

    if (this.cell.newValue !== '') {
      let dateAndTime = this.getValue().split('\n');
      let date = dateAndTime[0].split('-');
      let time = dateAndTime[1].split(':');

      this.dateTime = new Date(
        parseInt(date[2]),
        parseInt(date[1]) - 1,
        parseInt(date[0]),
        parseInt(time[0]),
        parseInt(time[1]),
        0
      );
    }
    this.cdr.detectChanges();
  }

  updateValue() {
    if (this.formGroup.controls['value'].hasError('pastDatesValidator')) {
      DateTimeValidator.isDateValid[`${this.cell.getTitle()}`] = false;
    } else {
      DateTimeValidator.isDateValid[`${this.cell.getTitle()}`] = true;
    }

    if (this.dateTime != undefined) {
      this.cell.newValue =
        moment(this.dateTime).format().toString().slice(0, 17) + '00.000Z';
    }
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
