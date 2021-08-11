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

  validate(event: any): void {
    let theEvent = event || window.event;
    let key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    let regex = /[0-9]|[.]/;
    if (!regex.test(key)) {
      theEvent.returnValue = false;
      if (theEvent.preventDefault) theEvent.preventDefault();
    }
  }

  transformValue(element: any): void {
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

  updateValue(): void {
    this.cell.newValue = this.value;
  }

  getValue(): string {
    return this.htmlValue.nativeElement.innerText;
  }
}
