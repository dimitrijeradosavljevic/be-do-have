import {Component, EventEmitter, Input, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {PopoverController} from '@ionic/angular';
import {PageSideMenuDto} from '../../../_shared/models/PageSideMenuDto';
import {PagePopoverComponent} from '../page-popover/page-popover.component';


@Component({
  selector: 'app-page-list-item-side-menu',
  templateUrl: './page-list-item-side-menu.component.html',
  styleUrls: ['./page-list-item-side-menu.component.scss'],
})
export class PageListItemSideMenuComponent implements OnInit {

  @Input() page: PageSideMenuDto;
  pagePopover: any;

  constructor(private router: Router,
              private popoverController: PopoverController) { }

  ngOnInit() { }

  async presentPopover(ev: any) {
    const closePopoverEvent: EventEmitter<any> = new EventEmitter<any>();
    closePopoverEvent.subscribe(value => {
      this.hidePagePopover();
    });

    this.pagePopover = await this.popoverController.create({
      component: PagePopoverComponent,
      componentProps: {
        pageId: this.page.id,
        closePopoverEvent
      },
      event: ev,
      translucent: true
    });
    await this.pagePopover.present();
  }

  goToCreateSubPage(): void {
    this.router.navigate(['/pages'], { queryParams: { directParentId: this.page.id }});
  }

  hidePagePopover(): void {
    this.pagePopover.dismiss();
  }
}
