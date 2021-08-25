import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  AsyncValidatorFn,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { MyErrorStateMatcher } from '../custom/validators/error-state-matcher';
import { ConfirmedValidator } from '../custom/validators/confirmed.validator';
import { Login } from 'src/app/Models/login';
import { Registration } from 'src/app/Models/registration';

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.css'],
})
export class AuthFormComponent implements OnInit {
  authFormComponent = AuthFormComponent;
  static isSessionExpired = false;
  static isWrongLoginorPassword = false;
  static isEmailExist = false;
  loginForm!: FormGroup;
  registrationForm!: FormGroup;
  loading = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authenticationService: AuthService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      loginControls: this.formBuilder.array([
        new MyFormControl('login', {
          validators: [Validators.required],
        }),
        new MyFormControl('password', {
          validators: [Validators.required],
        }),
      ]),
    });

    this.registrationForm = this.formBuilder.group(
      {
        registerControls: this.formBuilder.array([
          new MyFormControl('email', {
            validators: [Validators.required, Validators.email],
          }),
          new MyFormControl('login', {
            validators: [Validators.required],
          }),
          new MyFormControl('password', {
            validators: [Validators.required, Validators.minLength(6)],
          }),
          new MyFormControl('confirmPassword', Validators.minLength(6)),
        ]),
      },
      {
        validators: [ConfirmedValidator.confirmedValidator(2, 3)],
      }
    );
  }

  login(formData: Login) {
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.authenticationService
      .login(formData)
      .subscribe({
        next: () => {
          this.router.navigate(['/user/main']);
        },
        error: (error) => {
          this.error = error;
          this.loading = false;
        },
      });
  }

  register(formData: Registration) {
    if (this.registrationForm.invalid) {
      return;
    }

    this.loading = true;
    this.authenticationService
      .register(formData)
      .subscribe({
        next: () => {
          this.router.navigate(['/user/main']);
        },
        error: (error) => {
          this.error = error;
          this.loading = false;
        },
      });
  }
}

export class MyFormControl extends FormControl {
  type!: string;
  placeholder!: string;
  matcher: MyErrorStateMatcher;
  isSubmitted: boolean = false;

  constructor(
    formState: any = null,
    validatorOrOpts?: ValidatorFn | AbstractControlOptions | ValidatorFn[],
    asyncValidator?: AsyncValidatorFn | AsyncValidatorFn[]
  ) {
    super(formState, validatorOrOpts, asyncValidator);
    this.matcher = new MyErrorStateMatcher(false);
  }
}
