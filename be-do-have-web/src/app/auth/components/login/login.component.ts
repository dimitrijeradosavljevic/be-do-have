import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { login } from '../../store/auth.actions';
import {Store} from '@ngrx/store';
import {State} from '../../../_shared/store';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  public loginForm: FormGroup;
  submitted: boolean = false;
  formActive = false;

  constructor(private formBuilder: FormBuilder,
              private store: Store<State>) {
  }

  ngOnInit() {
    this.buildLoginForm();
  }

  buildLoginForm(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [ Validators.required, Validators.email ] ],
      password: ['', [ Validators.required,  Validators.minLength(6) ] ]
    });
    this.formActive = true;
  }

  login(): void {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    this.store.dispatch(login({
      user: {
        email: this.loginForm.controls.email.value,
        password: this.loginForm.controls.password.value
      }
    }));
    this.loginForm.reset();
  }

  showPassword(password): void {
    password.type = password.type === 'password' ? 'text' : 'password';
  }
}
