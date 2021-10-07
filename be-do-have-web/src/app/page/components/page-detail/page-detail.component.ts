import { Component, EventEmitter, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { select, Store } from '@ngrx/store';

import { State } from '../../../_shared/store';
import { selectedOrganisation } from '../../../organisation/store/organisation.selectors';
import { authUser } from '../../../auth/store/auth.selectors';
import { addPageInTree } from '../../store/page.actions';

import { PageService } from '../../services/page.service';
import { ToastService } from '../../../_shared/services/toast.service';
import { PageSideMenuDto } from '../../../_shared/models/PageSideMenuDto';
import { PageSearchParameters } from '../../../_shared/models/PageSearchParameters';
import { Page } from '../../../_shared/models/Page';
import { ModalController, PopoverController } from '@ionic/angular';
import { IconPickerComponent } from '../../../_shared/components/icon-picker/icon-picker.component';
import { TagService } from '../../../tag/services/tag.service';
import { Tag } from '../../../_shared/models/Tag';
import {TagsPickerComponent} from '../../../tag/components/tags-picker/tags-picker.component';
import {User} from "../../../_shared/models/User";
import * as moment from 'moment';

@Component({
  selector: 'app-page-detail',
  templateUrl: './page-detail.component.html',
  styleUrls: ['./page-detail.component.scss'],
})
export class PageDetailComponent implements OnInit {

  editorForm: FormGroup;
  editorFormActive = false;
  page: Page;
  authUser: User;
  submitted = false;
  loading = false;
  directParentId?;
  pageFilter: FormControl;
  highlightedContent;
  minutesRead = 0;
  iconPickerPopover: any;
  // content: string;

  editorStyle: any = {
    minHeight: '100px',
    border: '0px'
  };

  tagPickerPopover: any;
  selectedTags: Tag[] = [];

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private popoverController: PopoverController,
              private modalController: ModalController,
              private pageService: PageService,
              private toastService: ToastService,
              private tagService: TagService,
              private store: Store<State>) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  private initializeComponent(): void {

    this.store.pipe(select(authUser)).subscribe(user => this.authUser = user);

    this.route.paramMap.subscribe(params => {
      if (params.has('pageId')) {
        const pageId = params.get('pageId');
        this.fetchPage(pageId);
        // this.buildFilter();
      } else {
        this.buildEditorForm(null);
      }
    });

    this.route.queryParamMap.subscribe(query => {
      if (query.has('directParentId')) {
        this.directParentId = query.get('directParentId');
      }
    });
  }

  // buildFilter() {
  //   this.pageFilter = new FormControl('');
  //   this.pageFilter.valueChanges
  //     .pipe(
  //       debounceTime(350),
  //       distinctUntilChanged())
  //     .subscribe(value => {
  //       this.search();
  //     });
  // }

  private buildEditorForm(page?: Page): void {
    this.editorForm = this.formBuilder.group({
      id: [page ? page.id : null],
      title: [page ? page.title : '', Validators.required],
      content: [page ? page.content : '', Validators.required],
      iconName: [page ? page.iconName: null],
      iconColor: [page ? page.iconColor: null]
    });

    this.editorFormActive = true;
  }

  private onSubmit() {

    if (this.editorForm.invalid) {
      this.submitted = true;
      return;
    }

    /*
    const val = [
      "Avengers: Endgame",
      "Captain Marvel",
      "Ant-man and the Wasp",
      "Avengers: Infinity War",
      "Black Panther",
      "Thor: Ragnarok",
      "Spider-Man: Homecoming",
      "Guardians of the Galaxy Vol 2",
      "Doctor Strange",
      "Captain America: Civil War",
      "Ant-Man", "year",
      "Avengers: Age of Ultron",
      "Guardians of the Galaxy",
      "Captain America: The Winter Soldier",
      "Thor: The Dark World",
      "Iron Man 3",
      "Marvel's The Avengers",
      "Captain America: The First Avenger",
      "Thor",
      "Iron Man 2",
      "The Incredible Hulk",
      "Iron Man"
    ];
    const page = this.editorForm.value as Page;
    this.store.pipe(select(selectedOrganisation)).subscribe(organisation => page.organisationId = organisation.id);
    this.store.pipe(select(authUser)).subscribe(user => page.userId = user.id);
    val.forEach(v => {

      page.directPageId = this.directParentId;
      page.content = v;
      this.pageService.store(page).subscribe(
        result => {
          this.toastService.showSuccess('Page successfully created');
        }
      );
    });
    return;
     */

    const page = this.editorForm.value as Page;
    page.directPageId = this.directParentId;
    page.tags = this.selectedTags;

    this.loading = true;
    if (page.id) {
      this.pageService.update(page).subscribe(
        result => {
          this.toastService.showSuccess('Page successfully updated');
        }
      );
    } else {
      this.store.pipe(select(selectedOrganisation)).subscribe(organisation => page.organisationId = organisation.id);
      page.userId = this.authUser.id;
      this.pageService.store(page).subscribe(
        result => {

          const newNode = { id: result.id, title: result.title, open: false,  descedants: [] } as PageSideMenuDto;
          this.store.dispatch(addPageInTree({page: newNode, directParentId: this.directParentId}));

          this.editorForm.controls.id.setValue(result.id);
          this.toastService.showSuccess('Page successfully created');
        }
      );
    }
  }

  private fetchPage(pageId) {
    this.pageService.show(pageId).subscribe(
      result => {
        this.buildEditorForm(result);
        this.page = result;
        this.page.createdAt = moment(this.page.createdAt).format('DD. MMM. yyyy');
        this.selectedTags = result.tags;
        this.minutesRead = Math.ceil(this.page.content.split(' ').length / 275);
      }
    );
  }

  private triggerSubmit(event: KeyboardEvent): void {

    if (event.ctrlKey && event.key === 'Enter') {
      this.onSubmit();
    }
  }


  // private search() {
  //   if (this.pageFilter.value == '') {
  //     this.editorForm.controls.content.setValue(this.content);
  //     return;
  //   }
  //   const parameters = {
  //     pageId: this.page.id,
  //     term: this.pageFilter.value
  //   } as PageSearchParameters;
  //
  //   this.pageService.searchPage(parameters).subscribe(
  //     result => {
  //       this.highlightedContent = JSON.stringify(result);
  //       this.editorForm.controls.content.setValue(this.highlightedContent);
  //       console.log(this.highlightedContent);
  //     },
  //     error => {
  //
  //     }
  //   );
  // }

  onContentChanged($event: any) {
    if ($event.text === '\n') {
      this.minutesRead = 0;
    }
    else {
      this.minutesRead = Math.ceil($event.text.split(' ').length / 275);
    }
  }

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

  async openIconPicker() {
    if (this.page && this.page.user.id !== this.authUser.id) {
      return;
    }

    const pickIcon: EventEmitter<any> = new EventEmitter<any>();
    pickIcon.subscribe(value => {
      this.editorForm.controls.iconName.setValue(value.name);
      this.editorForm.controls.iconColor.setValue(value.color);
    });

    const closePopover: EventEmitter<any> = new EventEmitter<any>();
    closePopover.subscribe(() => {
      this.iconPickerPopover.dismiss();
    });

    this.iconPickerPopover = await this.modalController.create({
      component: IconPickerComponent,
      componentProps: {
        pickIcon,
        closePopover
      }
    });

    return await this.iconPickerPopover.present();
  }


  tagDeSelected(tagId: number) {
    this.selectedTags = this.selectedTags.filter(t => t.id !== tagId);
  }

}

