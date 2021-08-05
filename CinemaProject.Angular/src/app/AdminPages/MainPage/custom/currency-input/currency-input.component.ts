import { CurrencyPipe } from '@angular/common';
import {
  Component,
  ElementRef,
  AfterViewInit,
  ViewChild,
  ChangeDetectorRef,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DefaultEditor } from 'ng2-smart-table';

@Component({
  selector: 'app-currency-input',
  templateUrl: './currency-input.component.html',
  styleUrls: ['./currency-input.component.css'],
})
export class CurrencyInputComponent
  extends DefaultEditor
  implements AfterViewInit
{
  @ViewChild('htmlValue') htmlValue!: ElementRef;
  value!: string;
  formGroup: FormGroup;

  constructor(
    private currencyPipe: CurrencyPipe,
    private cdr: ChangeDetectorRef
  ) {
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

  transformValue(element: any) {
    if (this.value.replace(/\s/g, '').match(/[-]{0,1}[\d]*[,.]{0,1}[\d]+/g)) {
      this.value = new Intl.NumberFormat('fr-BR', {
        style: 'currency',
        currency: 'BYN',
      }).format(
        parseFloat(
          parseFloat(
            this.value
              .replace(/\s/g, '')
              .match(/[-]{0,1}[\d]*[,.]{0,1}[\d]+/g)!
              .join()
              .replace(',', '.')
          ).toFixed(2)
        )
      );

      element.target.value = this.value;
    } else {
      this.value = '';
      element.target.value = '';
    }
  }

  updateValue() {
    this.cell.newValue = this.value;
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
