import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {select, Store} from '@ngrx/store';
import {debounceTime, distinctUntilChanged} from 'rxjs/operators';

import {addOrganisation, editOrganisation} from '../../store/organisation.actions';

import {OrganisationService} from '../../services/organisation.service';

import {State} from '../../../_shared/store';
import {User} from '../../../_shared/models/User';
import {Organisation} from '../../../_shared/models/Organisation';
import {authUser} from "../../../auth/store/auth.selectors";
import {OrganisationInviteService} from "../../../organisation-invite/services/organisation-invite.service";


@Component({
  selector: 'app-organisation-editor',
  templateUrl: './organisation-editor.component.html',
  styleUrls: ['./organisation-editor.component.scss'],
})
export class OrganisationEditorComponent implements OnInit {

  editorForm: FormGroup;
  editorFormActive = false;
  submitted = false;
  loading = false;
  userFilter: FormControl;
  @Input() organisationId: number;
  @Output() closeModalEvent: EventEmitter<any> = new EventEmitter<any>();
  paginationConfig = {
    pageSize: 10,
    pageIndex: 1,
    total: undefined
  };
  users: User[];

  constructor(private formBuilder: FormBuilder,
              private store: Store<State>,
              private organisationService: OrganisationService,
              private organisationInviteService: OrganisationInviteService) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    if (this.organisationId) {
      this.buildFilter();
      this.fetchOrganisation();
    } else {
      this.buildEditorForm(null);
    }
  }

  onSubmit() {

    if (this.editorForm.invalid) {
      this.submitted = true;
      return;
    }

    const organisation = this.editorForm.value as Organisation;

    this.loading = true;
    if (this.organisationId) {
      this.store.dispatch(editOrganisation({organisation, closeModalEvent: this.closeModalEvent}));
    } else {
      this.store.dispatch(addOrganisation({organisation, closeModalEvent: this.closeModalEvent}));

    }
    this.editorForm.reset();
    this.closeModalEvent.emit();
  }

  fetchOrganisation() {
    this.organisationService.show(this.organisationId).subscribe(
      result => this.buildEditorForm(result),
      error => {
        // TODO cover-me
      }
    );
  }

  buildEditorForm(organisation?: Organisation) {
    this.editorForm = this.formBuilder.group({
      id: [organisation ? organisation.id : null],
      name: [organisation ? organisation.name : '', Validators.required]
    });

    this.editorFormActive = true;
  }

  buildFilter() {

    this.userFilter = new FormControl('');
    this.userFilter.valueChanges
      .pipe(
        debounceTime(350),
        distinctUntilChanged())
      .subscribe(value => {
        this.search();
      });
    this.search();
  }

  search() {
    this.organisationService.fetchNonMembers(this.organisationId, this.userFilter.value, this.paginationConfig.pageIndex, this.paginationConfig.pageSize).subscribe(
      result => {
        this.users = result.items;
        this.paginationConfig.total = result.total;
      });
  }

  changePage(next: boolean) {
    this.paginationConfig.pageIndex = (next ? this.paginationConfig.pageIndex + 1 : this.paginationConfig.pageIndex - 1);
    this.search();
  }

  inviteUserToOrganisation(userId: number) {
    let currentUserId;
    this.store.pipe(select(authUser)).subscribe(user => currentUserId = user.id);
    this.organisationInviteService.invite(this.organisationId, currentUserId, userId).subscribe(
      result => {
        this.users = this.users.filter(u => u.id !== userId);
      });
  }
}
