import {Component, Input, OnInit} from '@angular/core';

import { OrganisationService } from '../../services/organisation.service';
import { User } from '../../../_shared/models/User';


@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.scss'],
})
export class MemberListComponent implements OnInit {

  @Input() organisationId;
  loading = true;
  members: User[];
  paginationConfig = {
    pageSize: 10,
    pageIndex: 1,
    total: undefined
  };

  constructor(private organisationService: OrganisationService) { }

  ngOnInit() {
    this.fetchMembers();
  }

  fetchMembers() {
    this.loading = true;
    this.organisationService.fetchMembers(this.organisationId, this.paginationConfig.pageIndex, this.paginationConfig.pageSize).subscribe(
      result => {
        this.members = result.items;
        this.paginationConfig.total = result.total;
      },
      error => {

      },
      () => this.loading = false
    );
  }

  changePage(next: boolean) {
    this.paginationConfig.pageIndex = (next ? this.paginationConfig.pageIndex + 1 : this.paginationConfig.pageIndex - 1);
    this.fetchMembers();
  }
}
