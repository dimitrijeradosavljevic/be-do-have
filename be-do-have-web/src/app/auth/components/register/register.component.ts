import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { Store } from '@ngrx/store';

import { State } from '../../../_shared/store';
import { register } from '../../store/auth.actions';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  registerFormActive = false;
  submitted = false;

  constructor(private formBuilder: FormBuilder,
              private store: Store<State>) {
  }


  ngOnInit(): void {

    this.buildRegisterForm();
  }

  buildRegisterForm(): void {

    this.registerForm = this.formBuilder.group({
      email: ['', [ Validators.required, Validators.email ] ],
      password: ['', [ Validators.required, Validators.minLength(6) ] ],
      confirmedPassword: ['', [ Validators.required] ]
    }, {
      validators: [
        this.strongPassword,
        this.passwordsMatch
      ]
    });

    this.registerFormActive = true;
  }

  strongPassword(registerForm: FormGroup): ValidationErrors | null {

    const password           = registerForm.controls.password.value;
    const hasNonAlphanumeric = /\W/;
    const hasNumber          = /\d/;
    const hasUppercase       = /[A-Z]/;

    return (hasNonAlphanumeric.test(password) && hasNumber.test(password) && hasUppercase.test(password) ? null :
      {passwordNotStrong: true});
  }

  passwordsMatch(registerForm: FormGroup): ValidationErrors | null {

    const password     = registerForm.controls.password.value;
    const confirmation = registerForm.controls.confirmedPassword.value;

    return (password === confirmation ? null : {passwordsNotMatched: true});
  }

  register(): void {

    this.submitted = true;
    if (!this.registerForm.valid) {
      return;
    }

    this.store.dispatch(register({
      user: {
        email:             this.registerForm.controls.email.value,
        password:          this.registerForm.controls.password.value,
        confirmedPassword: this.registerForm.controls.confirmedPassword.value
      }
    }));
    this.registerForm.reset();
  }

  showPassword(password): void {
    password.type = password.type === 'password' ? 'text' : 'password';
  }

}
