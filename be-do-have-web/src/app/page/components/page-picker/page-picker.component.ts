import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { ToastService } from '../../../_shared/services/toast.service';
import { PageService } from '../../services/page.service';
import { Page } from '../../../_shared/models/Page';

@Component({
  selector: 'app-page-picker',
  templateUrl: './page-picker.component.html',
  styleUrls: ['./page-picker.component.scss'],
})
export class PagePickerComponent implements OnInit {

  pages: Page[] = [];
  loading: boolean;
  titleFilter: FormControl;
  titleFilterActive = false;
  @Input() pickedPages: Page[];
  @Output() pickPage: EventEmitter<Page> = new EventEmitter<Page>();

  paginationConfig = {
    pageSize: 5,
    pageIndex: 1,
    total: null,
  };

  constructor(private toastService: ToastService,
              private pageService: PageService) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent(): void {

    this.loading = true;
    this.buildFilter();
    this.fetchPages();
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
      });
    this.titleFilterActive = true;
  }


  fetchPages() {

    this.pageService.pagesForPicker(this.titleFilter.value, this.paginationConfig.pageIndex,
      this.paginationConfig.pageSize).subscribe(
      result => {
        this.pages = result.items.filter(page => !this.pickedPages.find(p => p.id === page.id));
        this.paginationConfig.total = result.total;
      },
      error => {

      },
      () => this.loading = false
    );
  }

  pick(page: any) {
    this.pickPage.emit(page);
    this.pages = this.pages.filter(p => p.id !== page.id);
  }

  changePage(next: boolean) {
    this.paginationConfig.pageIndex = (next ? this.paginationConfig.pageIndex + 1 : this.paginationConfig.pageIndex - 1);
    this.fetchPages();
  }
}
