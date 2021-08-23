import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray } from '@angular/forms';
import { AbstractControl } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { MyFormControl } from '../../auth-form/auth-form.component';
import { MyErrorStateMatcher } from '../validators/error-state-matcher';

@Component({
  selector: 'custom-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css'],
})
export class FormComponent implements OnInit {
  @Input() formGroup!: FormGroup;
  @Output() onSubmit = new EventEmitter<any>();
  controls: MyFormControl[] = [];
  confirmPasswordControl!: MyFormControl;
  nameOfForm!: string;

  constructor() {}

  ngOnInit(): void {
    //Get name of control as FormArray
    Object.keys(this.formGroup.controls).forEach((key: string) => {
      this.nameOfForm = key;
    });

    //Get Controls of FormArray
    let myControls = this.getControls();

    Object.keys(myControls).forEach((index: string) => {
      //Select a control
      let myControl = myControls[parseInt(index)] as MyFormControl;

      //Matcher for internal validation of form-field component
      myControl.matcher = new MyErrorStateMatcher(false);

      //Select type of input
      if (myControl.value.toLowerCase().includes('password')) {
        myControl.type = 'password';
      } else {
        myControl.type = 'text';
      }

      if (myControl.value === 'confirmPassword') {
        this.confirmPasswordControl = myControl;
      }

      //Set placeholder from value of control of FormArray
      myControl.placeholder =
        myControl.value[0].toUpperCase() + myControl.value.slice(1);

      myControl.setValue('');
      this.controls.push(myControl);
    });
  }

  private getControls(): AbstractControl[] {
    return (this.formGroup.get(this.nameOfForm) as FormArray).controls;
  }

  submit() {
    let myControls = this.getControls();

    Object.keys(myControls).forEach((index: string) => {
      let myControl = myControls[parseInt(index)] as MyFormControl;

      myControl.isSubmitted = true;
      if (myControl.invalid) {
        myControl.matcher.isValid = true;
      }
    });

    let formData: { [key: string]: any } = {};
    this.controls.forEach((control) => {
      formData[`${control.placeholder.toLowerCase()}`] = control.value;
    });

    this.onSubmit.emit(formData);
  }
}
