import {Component, OnInit} from '@angular/core';
import {select, Store} from '@ngrx/store';
import {State} from '../_shared/store';
import {authUser, isLoggedIn} from "../auth/store/auth.selectors";
import {User} from "../_shared/models/User";

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage implements OnInit {

  user: User;

  constructor(private store: Store<State>) {}

  ngOnInit() {
    this.store
      .pipe(select(authUser))
      .subscribe(user => this.user = user);
  }

}
