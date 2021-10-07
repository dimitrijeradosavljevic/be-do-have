import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

import { Router } from '@angular/router';
import {PageService} from '../../services/page.service';
import {ToastService} from '../../../_shared/services/toast.service';
import {PagePickerComponent} from "../page-picker/page-picker.component";
import {PopoverController} from "@ionic/angular";
import {PageMoveComponent} from "../page-move/page-move.component";
import {fetchPagesTree, fetchPagesTreeSuccess} from "../../store/page.actions";
import {select, Store} from "@ngrx/store";
import {State} from "../../../_shared/store";
import {selectedOrganisation} from "../../../organisation/store/organisation.selectors";
import {selectOrganisation} from "../../../organisation/store/organisation.actions";
import {pages} from "../../store/page.selectors";


@Component({
  selector: 'app-page-popover',
  templateUrl: './page-popover.component.html',
  styleUrls: ['./page-popover.component.scss'],
})
export class PagePopoverComponent implements OnInit {

  @Input() pageId;
  @Output() closePopoverEvent: EventEmitter<any> = new EventEmitter<any>();
  pageMovePopover: any;

  constructor(private router: Router,
              private store: Store<State>,
              private popoverController: PopoverController,
              private pageService: PageService,
              private toastService: ToastService) { }

  ngOnInit() {}

  goToCreateSubPage(): void {
    this.router.navigate(['/pages'], { queryParams: { directParentId: this.pageId }});
    this.closePopoverEvent.emit();
  }

  trashPage() {
    this.pageService.trash(this.pageId, true).subscribe(
      result => {
        this.toastService.showSuccess('Page successfully deleted.');
        let pagesFromState;
        this.store.pipe(select(pages)).subscribe(pagesData => pagesFromState = pagesData);
        const sideMenuPages = this.pageService.copyTree(pagesFromState);
        const deletedNode = this.pageService.deleteNode(sideMenuPages, this.pageId);
        this.store.dispatch(fetchPagesTreeSuccess({ pages: deletedNode }));
      },
      error => {
        this.toastService.showError(error.message);
      },
      () => this.closePopoverEvent.emit()
    );
  }

  async openPageMove() {
    const pickPageEvent: EventEmitter<any> = new EventEmitter<any>();
    pickPageEvent.subscribe(value => {

      let organisationId;
      this.store.pipe(select(selectedOrganisation)).subscribe(organisation => organisationId = organisation.id);
      if (value.type === 'page') {
        this.pageService.movePage(this.pageId, value.id).subscribe(
          result => {
            this.store.dispatch(fetchPagesTree({ organisationId }));
          }
        );
      }
      else {
        this.pageService.movePageUnderOrganisation(this.pageId, value.id).subscribe(
          result => {
            this.store.dispatch(fetchPagesTree({ organisationId }));
          }
        );
      }
      this.closePopoverEvent.emit();
      this.pageMovePopover.dismiss();
    });

    this.pageMovePopover = await this.popoverController.create({
      component: PageMoveComponent,
      componentProps: {
        pickPage: pickPageEvent,
        pageId: this.pageId
      },
      translucent: true
    });
    await this.pageMovePopover.present();
  }
}
