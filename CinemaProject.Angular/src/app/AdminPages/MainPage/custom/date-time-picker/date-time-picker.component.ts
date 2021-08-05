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

@Component({
  selector: 'app-date-time-picker',
  templateUrl: './date-time-picker.component.html',
  styleUrls: ['./date-time-picker.component.css'],
})
export class DateTimePickerComponent
  extends DefaultEditor
  implements AfterViewInit
{
  @ViewChild('picker') picker: any;
  @ViewChild('htmlValue') htmlValue!: ElementRef;
  @Input() dateTime!: Date;
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
      let dateAndTime = this.getValue().split('\n');
      let date = dateAndTime[0].split('-');
      let time = dateAndTime[1].split(':');

      this.dateTime = new Date(
        parseInt(date[2]),
        parseInt(date[0]) - 1,
        parseInt(date[1]),
        parseInt(time[0]),
        parseInt(time[1]),
        0
      );
    }
    this.cdr.detectChanges();
  }

  updateValue() {
    if (this.dateTime != undefined) {
      this.cell.newValue = new Date(
        this.dateTime.getTime() - this.dateTime.getTimezoneOffset() * 60000
      ).toISOString();
    }
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
