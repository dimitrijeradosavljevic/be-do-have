import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree} from '@angular/router';
import { select, Store } from '@ngrx/store';

import { State } from '../store';
import { AuthService } from '../../auth/services/auth.service';
import { isLoggedIn } from '../../auth/store/auth.selectors';
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  private isUserAuth: boolean;

  constructor(
    private store: Store<State>
  ) {
    this.store
      .pipe(select(isLoggedIn))
      .subscribe(loggedIn => this.isUserAuth = loggedIn);
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.isUserAuth;
  }
}
