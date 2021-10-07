import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {Router} from '@angular/router';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MenuController, PopoverController } from '@ionic/angular';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { PagePickerComponent } from '../page-picker/page-picker.component';

import { PageService } from '../../services/page.service';
import { PageSearch } from '../../../_shared/models/PageSearch';
import { PageSearchParameters } from '../../../_shared/models/PageSearchParameters';
import { Page } from '../../../_shared/models/Page';
import {select, Store} from '@ngrx/store';
import { selectedOrganisation } from '../../../organisation/store/organisation.selectors';
import {State} from '../../../_shared/store';
import {Tag} from '../../../_shared/models/Tag';
import {TagsPickerComponent} from "../../../tag/components/tags-picker/tags-picker.component";

@Component({
  selector: 'app-page-search',
  templateUrl: './page-search.component.html',
  styleUrls: ['./page-search.component.scss'],
})
export class PageSearchComponent implements OnInit {

  keyword = 'name';
  suggestions: any[] = [];
  pages: PageSearch[] = [];
  pickedPages: Page[] = [];
  pageFilter: FormControl;
  filterForm: FormGroup;
  loading = false;
  showFilters = false;
  pagePickerPopover: any;
  selectedTags: Tag[] = [];
  tagPickerPopover: any;
  @Output() closeModalEvent: EventEmitter<any> = new EventEmitter<any>();

  constructor(private router: Router,
              private formBuilder: FormBuilder,
              private store: Store<State>,
              private popoverController: PopoverController,
              private pageService: PageService) {
  }

  ngOnInit() {
    this.buildFilterForm();
  }

  buildFilterForm() {
    this.filterForm = this.formBuilder.group({
      term: [''],
      titleOnly: [false],
      createdAtStart: [null],
      createdAtEnd: [null],
      updatedAtStart: [null],
      updatedAtEnd: [null],
      author: [null]
    });

    this.filterForm.valueChanges
      .pipe(
        debounceTime(350),
        distinctUntilChanged())
      .subscribe(value => {
        this.search();
        this.autoCompleteSuggest();
      });
  }

  search() {
    this.loading = true;
    const parameters = this.filterForm.value as PageSearchParameters;
    parameters.tagIds = this.selectedTags.map(tag => tag.id);
    this.store.pipe(select(selectedOrganisation)).subscribe(organisation => parameters.organisationId = organisation.id);
    if (this.pickedPages.length > 0) {
      parameters.inPagesSearch = this.pickedPages.map(picked => picked.id);
    }

    this.pageService.search(parameters).subscribe(
      result => {
        this.pages = result;
      },
      error => {

      },
      () => this.loading = false
    );
  }

  autoCompleteSuggest() {
    this.pageService.autoCompleteSuggest(this.filterForm.controls.term.value).subscribe(
      result => {
        // this.suggestions = result.map(r => {
        //   return {
        //     id: 1,
        //     name: r
        //   };
        // });
        this.suggestions = result;
      },
      error => {

      }
    );
  }

  gotoPage(id: any) {
    this.closeModalEvent.emit();
    this.router.navigate([`/pages/${id}`]);
  }

  async openPagePicker($event: any) {
    const pickPageEvent: EventEmitter<any> = new EventEmitter<any>();
    pickPageEvent.subscribe(value => {
      this.pickedPages.push(value);
    });

    this.pagePickerPopover = await this.popoverController.create({
      component: PagePickerComponent,
      componentProps: {
        pickedPages: this.pickedPages,
        pickPage: pickPageEvent
      },
      event: $event,
      translucent: true
    });
    await this.pagePickerPopover.present();
  }

  unpickPage(id: any) {
    this.pickedPages = this.pickedPages.filter(p => p.id !== id);
  }

  onChangeSearch($event: any) {
    this.filterForm.controls.term.setValue($event);
  }

  suggestionsFilter($event: any) {
    return $event;
  }

  customFilter = (items) => items;

  async openTagPicker() {
    const tagPickedEvent: EventEmitter<Tag> = new EventEmitter<Tag>();
    tagPickedEvent.subscribe(tag => {
      this.selectedTags.push(tag);
      if (this.selectedTags.length >= 5) {
        this.tagPickerPopover.dismiss();
      }
    });

    this.tagPickerPopover = await this.popoverController.create({
      component: TagsPickerComponent,
      componentProps: {
        tagPicked: tagPickedEvent,
        withCreate: true
      }
    });

    return await this.tagPickerPopover.present();
  }

  tagDeSelected(tagId: number) {
    this.selectedTags = this.selectedTags.filter(t => t.id !== tagId);
  }
}
