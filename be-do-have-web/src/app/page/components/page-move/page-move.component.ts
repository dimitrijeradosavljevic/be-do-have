import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { Store } from '@ngrx/store';
import { State } from '../../../_shared/store';

import { ToastService } from '../../../_shared/services/toast.service';
import { PageService } from '../../services/page.service';
import { OrganisationService } from '../../../organisation/services/organisation.service';

import { Page } from '../../../_shared/models/Page';
import { Organisation } from '../../../_shared/models/Organisation';

@Component({
  selector: 'app-page-move',
  templateUrl: './page-move.component.html',
  styleUrls: ['./page-move.component.scss'],
})
export class PageMoveComponent implements OnInit {


  organisations: Organisation[] = [];
  pages: Page[] = [];
  loading = false;
  titleFilter: FormControl;
  titleFilterActive = false;
  selectedOrganisationId;
  @Input() pageId: number;
  @Output() pickPage: EventEmitter<any> = new EventEmitter<any>();

  paginationConfig = {
    pageSize: 3,
    pageIndex: 1,
    total: undefined,
  };

  constructor(private store: Store<State>,
              private toastService: ToastService,
              private pageService: PageService,
              private organisationService: OrganisationService) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent(): void {
    this.loading = true;
    this.buildFilter();
    this.fetchPages();
    this.fetchOrganisations();
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
        this.loading = true;
        this.fetchPages();
        this.fetchOrganisations();
      });
    this.titleFilterActive = true;
  }


  onLoadTrashed(event): void {
    this.paginationConfig.pageIndex++;
    this.fetchPages();
  }

  fetchPages() {

    this.pageService.pagesForPicker(this.titleFilter.value, this.paginationConfig.pageIndex, this.paginationConfig.pageSize)
                    .subscribe(
      result => {
        this.pages = result.items.filter(p => p.id !== this.pageId);
      },
      error => {

      },
      () => this.loading = false
    );
  }

  fetchOrganisations() {
    this.organisationService.index(this.titleFilter.value, this.paginationConfig.pageIndex, this.paginationConfig.pageSize)
      .subscribe(
        result => {
          this.organisations = result.items;
        },
        error => {

        },
        () => this.loading = false
      );
  }

  changePage(next: boolean) {
    this.paginationConfig.pageIndex = (next ? this.paginationConfig.pageIndex + 1 : this.paginationConfig.pageIndex - 1);
    this.fetchPages();
    this.fetchOrganisations();
  }

  pick(id: number, type: string) {
    const emit = {
      id,
      type
    };
    this.pickPage.emit(emit);
  }
}
