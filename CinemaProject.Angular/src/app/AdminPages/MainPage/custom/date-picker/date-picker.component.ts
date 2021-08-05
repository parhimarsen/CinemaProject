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

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.css'],
})
export class DatePickerComponent
  extends DefaultEditor
  implements AfterViewInit
{
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
      this.cell.newValue = this.releaseDate.toISOString();
    }
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
