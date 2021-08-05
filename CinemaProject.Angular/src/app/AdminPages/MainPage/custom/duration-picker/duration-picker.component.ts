import {
  Component,
  AfterViewInit,
  Input,
  Output,
  EventEmitter,
  ChangeDetectorRef,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { DefaultEditor } from 'ng2-smart-table';

@Component({
  selector: 'app-duration-picker',
  templateUrl: './duration-picker.component.html',
  styleUrls: ['./duration-picker.component.css'],
})
export class DurationPickerComponent
  extends DefaultEditor
  implements AfterViewInit
{
  @ViewChild('htmlValue') htmlValue!: ElementRef;
  @Input() durationString: string;
  @Input() disabled: any;

  @Output() durationStringChange: EventEmitter<string> =
    new EventEmitter<string>();

  hours: any;
  minutes: any;
  seconds: any;

  constructor(private cdr: ChangeDetectorRef) {
    super();
    this.durationString = '00:00:00';
  }

  ngAfterViewInit(): void {
    if (this.cell.newValue !== '') {
      this.durationString = this.getValue();
    }
    if (this.durationString) {
      let splitDuration = this.durationString.split(':');

      this.hours = splitDuration[0];
      this.minutes = splitDuration[1];
      this.seconds = splitDuration[2];
    }
    this.cdr.detectChanges();
  }

  updateValue() {
    const duration =
      '0.' +
      this.durationString.slice().replace('/undefined/g', '00') +
      '.0000';

    this.cell.newValue = duration;
  }

  check(type: any) {
    if (type === 'hours') {
      this.hours = this.getValidValue(this.hours, 23);
    } else if (type === 'minutes') {
      this.minutes = this.getValidValue(this.minutes, 59);
    } else if (type === 'seconds') {
      this.seconds = this.getValidValue(this.seconds, 59);
    }

    this.durationString = `${this.hours}:${this.minutes}:${this.seconds}`;
    this.durationStringChange.emit(this.durationString);
  }

  getValidValue(value: any, max: any) {
    let n;
    if (/^\d+$/.test(value)) {
      n = parseInt(value);
      n = Math.max(0, n);
      n = Math.min(max, n);
      if (n < 10) return new String('0' + n);
      else return new String(n);
    } else {
      return new String('0');
    }
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
