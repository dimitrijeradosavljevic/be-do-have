import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';

import { GuestGuard } from '../_shared/guards/guest.guard';


const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [ GuestGuard ]
  },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [ GuestGuard ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
