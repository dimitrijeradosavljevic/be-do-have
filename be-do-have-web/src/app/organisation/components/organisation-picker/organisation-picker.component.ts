import { Component, EventEmitter, OnInit } from '@angular/core';
import { ModalController, PopoverController } from '@ionic/angular';
import { select, Store } from '@ngrx/store';
import { Organisation } from '../../../_shared/models/Organisation';
import { State } from '../../../_shared/store';
import { organisations } from '../../store/organisation.selectors';
import { selectOrganisation } from '../../store/organisation.actions';
import { OrganisationPopoverComponent } from '../organisation-popover/organisation-popover.component';

@Component({
  selector: 'app-organisation-picker',
  templateUrl: './organisation-picker.component.html',
  styleUrls: ['./organisation-picker.component.scss'],
})
export class OrganisationPickerComponent implements OnInit {


  selected = {
    id: -1,
    name: 'Private'
  };
  organisations: Organisation[];
  organisationsSubscription$;
  organisationPopover: any;


  constructor(private store: Store<State>,
              private modalController: ModalController,
              private popoverController: PopoverController) { }

  ngOnInit() {
    this.organisationsSubscription$ = this.store.pipe(select(organisations))
      .subscribe(organisationsData => {
        this.organisations = organisationsData;
        if (this.selected.id != -1) {
          this.selected.name = this.organisations.find(o => o.id === this.selected.id).name;
        }
      });
  }

  private onOrganisationChange(data) {
    const organisation = this.organisations.find(o => o.id == data.detail.value);
    this.selected.id = organisation.id;
    this.selected.name = organisation.name;

    this.store.dispatch(selectOrganisation({ organisation }));
  }

  async presentOrganisationPopover(ev: any) {
    const closePopoverEvent: EventEmitter<any> = new EventEmitter<any>();
    closePopoverEvent.subscribe(value => {
      this.hideOrganisationPopover();
    });

    this.organisationPopover = await this.popoverController.create({
      component: OrganisationPopoverComponent,
      componentProps: {
        closePopoverEvent
      },
      translucent: true
    });

    await this.organisationPopover.present();
  }

  private async hideOrganisationPopover() {
    await this.organisationPopover.dismiss();
  }
}
