import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { PageService } from '../../services/page.service';

import { Page } from '../../../_shared/models/Page';
import { ToastService } from '../../../_shared/services/toast.service';
import { select, Store } from '@ngrx/store';
import { selectedOrganisation } from '../../../organisation/store/organisation.selectors';
import { State } from '../../../_shared/store';
import {fetchPagesTree} from "../../store/page.actions";

@Component({
  selector: 'app-page-trashed-popover',
  templateUrl: './page-trashed-popover.component.html',
  styleUrls: ['./page-trashed-popover.component.scss'],
})
export class PageTrashedPopoverComponent implements OnInit {

  pages: Page[] = [];
  loading: boolean;
  titleFilter: FormControl;
  titleFilterActive = false;
  organisationId: number;

  paginationConfig = {
    pageSize: 7,
    pageIndex: 1,
    total: null,
  };

  constructor(private store: Store<State>,
              private toastService: ToastService,
              private pageService: PageService) { }

  ngOnInit() {

    this.initializeComponent();
  }

  initializeComponent(): void {

    this.store.pipe(select(selectedOrganisation)).subscribe(organisation => this.organisationId = organisation.id);
    this.loading = true;
    this.buildFilter();
    this.fetchTrashed();
  }

  buildFilter() {
    this.titleFilter = new FormControl('');
    this.titleFilter.valueChanges
      .pipe(
        debounceTime(350),
        distinctUntilChanged())
      .subscribe(value => {
        this.paginationConfig.pageIndex = 1;
        this.pages = [];
        this.fetchTrashed();
      });
    this.titleFilterActive = true;
  }

  private fetchTrashed() {

    this.loading = true;
    this.pageService.trashed(this.titleFilter.value, this.organisationId, this.paginationConfig.pageIndex,
      this.paginationConfig.pageSize, 'id', 'asc').subscribe(
      result => {
        this.pages = result.items;
        this.paginationConfig.total = result.total;
      },
      error => {

      },
      () => this.loading = false
    );
  }


  private deletePage(pageId: number): void {
    this.pageService.destroy(pageId).subscribe(
      result => {
        this.toastService.showSuccess('Page successfully deleted');
        this.pages = this.pages.filter(p => p.id !== pageId);
      }
    );
  }

  private untrashPage(pageId: number): void {
    this.pageService.trash(pageId, false).subscribe(
      result => {
        this.store.dispatch(fetchPagesTree({ organisationId: this.organisationId }));
        this.toastService.showSuccess('Page successfully untrashed.');
        this.pages = this.pages.filter(p => p.id !== pageId);

      },
      error => {
        this.toastService.showError(error.message);
      }
    );
  }

  changePage(next: boolean) {
    this.paginationConfig.pageIndex = (next ? this.paginationConfig.pageIndex + 1 : this.paginationConfig.pageIndex - 1);
    this.fetchTrashed();
  }
}
