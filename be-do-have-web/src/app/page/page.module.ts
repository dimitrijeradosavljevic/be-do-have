import { NgModule } from '@angular/core';
import { SharedModule } from '../_shared/shared.module';
import {StoreModule} from '@ngrx/store';
import {EffectsModule} from '@ngrx/effects';

import { QuillModule } from 'ngx-quill';

import {pageFeatureKey, pageReducer } from './store/page.reducer';
import {PageEffects} from './store/page.effects';

import { PageRoutingModule } from './page-routing.module';

import { PageListSideMenuComponent } from './components/page-list-side-menu/page-list-side-menu.component';
import { PageListItemSideMenuComponent } from './components/page-list-item-side-menu/page-list-item-side-menu.component';
import { PageEditorComponent } from './components/page-editor/page-editor.component';
import { PageDetailComponent } from './components/page-detail/page-detail.component';
import { PagePopoverComponent } from './components/page-popover/page-popover.component';
import { PageTrashedPopoverComponent } from './components/page-trashed-popover/page-trashed-popover.component';
import { PageSearchComponent } from './components/page-search/page-search.component';
import { PagePickerComponent } from './components/page-picker/page-picker.component';
import { PageMoveComponent } from './components/page-move/page-move.component';


@NgModule({
  declarations: [
    PageListSideMenuComponent,
    PageListItemSideMenuComponent,
    PageEditorComponent,
    PageDetailComponent,
    PagePopoverComponent,
    PageTrashedPopoverComponent,
    PageSearchComponent,
    PagePickerComponent,
    PageMoveComponent
],
  imports: [
    SharedModule,

    QuillModule.forRoot(),


    StoreModule.forFeature(pageFeatureKey, pageReducer),
    EffectsModule.forFeature([PageEffects]),

    PageRoutingModule,


  ],
  exports: [
    PageListSideMenuComponent
  ]
})
export class PageModule { }
