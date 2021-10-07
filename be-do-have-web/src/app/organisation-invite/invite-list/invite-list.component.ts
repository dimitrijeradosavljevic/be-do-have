import { Component, OnInit } from '@angular/core';
import { OrganisationInviteService } from '../services/organisation-invite.service';
import { select, Store } from '@ngrx/store';
import { State } from '../../_shared/store';
import { authUser } from '../../auth/store/auth.selectors';
import { OrganisationInvite } from '../../_shared/models/OrganisationInvite';
import { becomeOrganisationMember } from '../../organisation/store/organisation.actions';

@Component({
  selector: 'app-invite-list',
  templateUrl: './invite-list.component.html',
  styleUrls: ['./invite-list.component.scss'],
})
export class InviteListComponent implements OnInit {

  loading = true;
  invites: OrganisationInvite[];
  paginationConfig = {
    pageSize: 10,
    pageIndex: 1,
    total: undefined
  };

  constructor(private store: Store<State>,
              private organisationInviteService: OrganisationInviteService) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.fetchInvites();
  }

  fetchInvites() {
    let authUserId;
    this.store.pipe(select(authUser)).subscribe(user => authUserId = user.id);
    this.loading = true;
    this.organisationInviteService.getInvites(authUserId, this.paginationConfig.pageIndex, this.paginationConfig.pageSize).subscribe(
      result => {
        this.invites = result.items;
      },
      error => {

      },
      () => this.loading = false
    );
  }

  changePage(next: boolean) {
    this.paginationConfig.pageIndex = (next ? this.paginationConfig.pageIndex + 1 : this.paginationConfig.pageIndex - 1);
    this.fetchInvites();
  }

  respondOnInvite(inviteId: number, response: boolean) {
    this.organisationInviteService.storeResponse(inviteId, response).subscribe(
      result => {
          this.invites = this.invites.filter(i => i.id !== inviteId);
          this.paginationConfig.total--;

          if (result) {
            this.store.dispatch(becomeOrganisationMember({organisation: result}));
          }
      }
    );
  }
}
