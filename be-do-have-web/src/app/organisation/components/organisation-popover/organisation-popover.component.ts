import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

import { OrganisationEditorComponent } from '../organisation-editor/organisation-editor.component';
import { ModalController } from '@ionic/angular';

import {select, Store} from '@ngrx/store';
import { State } from '../../../_shared/store';
import { selectedOrganisation } from '../../store/organisation.selectors';
import { MemberListComponent } from '../member-list/member-list.component';
import {authUser} from "../../../auth/store/auth.selectors";

@Component({
  selector: 'app-organisation-popover',
  templateUrl: './organisation-popover.component.html',
  styleUrls: ['./organisation-popover.component.scss'],
})
export class OrganisationPopoverComponent implements OnInit {

  selectedOrganisation;
  authUser;
  organisationEditorModal: any;
  @Output() closePopoverEvent: EventEmitter<any> = new EventEmitter<any>();

  constructor(private modalController: ModalController,
              private store: Store<State>) { }

  ngOnInit() {
      this.store.pipe(select(selectedOrganisation)).subscribe(organisation => {
        if (organisation) {
          this.selectedOrganisation = organisation;
        }
      });

    this.store.pipe(select(authUser)).subscribe(userData => this.authUser = userData);
  }


  hideOrganisationEditorModal() {
    this.organisationEditorModal.dismiss();
  }

  async openOrganisationEditorModal(edit: boolean) {
    this.closePopoverEvent.emit();

    const closeModalEvent: EventEmitter<any> = new EventEmitter<any>();
    closeModalEvent.subscribe(value => {
      this.hideOrganisationEditorModal();
    });

    this.organisationEditorModal = await this.modalController.create({
      component: OrganisationEditorComponent,
      componentProps: {
        closeModalEvent,
        organisationId: (this.selectedOrganisation && edit ? this.selectedOrganisation.id : null)
      }
    });

    return await this.organisationEditorModal.present();
  }

  async openMembersModal() {
    this.closePopoverEvent.emit();

    const membersModal = await this.modalController.create({
      component: MemberListComponent,
      componentProps: {
        organisationId: this.selectedOrganisation.id
      }
    });

    return await membersModal.present();
  }
}
