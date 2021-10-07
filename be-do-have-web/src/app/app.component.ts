import {Component, EventEmitter, OnInit} from '@angular/core';
import { ModalController, PopoverController} from "@ionic/angular";
import {NavigationEnd, Router} from "@angular/router";
import {PageTrashedPopoverComponent} from "./page/components/page-trashed-popover/page-trashed-popover.component";
import {OrganisationEditorComponent} from "./organisation/components/organisation-editor/organisation-editor.component";
import {PageSearchComponent} from "./page/components/page-search/page-search.component";
import {InviteListComponent} from "./organisation-invite/invite-list/invite-list.component";
import {Store} from "@ngrx/store";
import {State} from "./_shared/store";
import {logout} from "./auth/store/auth.actions";

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit {

  showMenu = false;
  quickSearchModal: any;

  constructor(private router: Router,
              private store: Store<State>,
              private modalController: ModalController,
              private popoverController: PopoverController) {

  }

  ngOnInit() {
    this.showSideMenu();
  }

  showSideMenu() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.showMenu = !(this.router.url == '/register' || this.router.url == '/login');
      }
    });
  }

  async presentPopover(ev: any) {
    const pagePopover = await this.popoverController.create({
      component: PageTrashedPopoverComponent,
    });
    await pagePopover.present();
  }

  async presentQuickSearchModal($event: any) {
    const closeModalEvent: EventEmitter<any> = new EventEmitter<any>();
    closeModalEvent.subscribe(value => {
      this.hideQuickSearchModal();
    });

    this.quickSearchModal = await this.modalController.create({
      component: PageSearchComponent,
      componentProps: {
        closeModalEvent
      }
    });

    return await this.quickSearchModal.present();
  }

  hideQuickSearchModal() {
    this.quickSearchModal.dismiss();
  }

  async showInvites() {
    const invitesPopover = await this.popoverController.create({
      component: InviteListComponent,
      cssClass: 'popover',
    });
    await invitesPopover.present();
  }

  logout() {
    this.store.dispatch(logout());
  }
}
