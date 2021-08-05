import {
  Component,
  AfterViewInit,
  ElementRef,
  ViewChild,
  ChangeDetectorRef,
} from '@angular/core';
import { DefaultEditor } from 'ng2-smart-table';

import { FormControl, FormGroup, Validators } from '@angular/forms';

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
      value: new FormControl(
        { value: '', disabled: false },
        Validators.required
      ),
    });
    this.formGroup.markAllAsTouched();
  }

  ngAfterViewInit(): void {
    if (this.cell.newValue !== '') {
      this.value = this.getValue();
    }
    this.cdr.detectChanges();
  }

  updateValue() {
    this.cell.newValue = this.value;
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
