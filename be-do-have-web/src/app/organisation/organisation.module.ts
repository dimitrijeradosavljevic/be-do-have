import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { SharedModule } from '../_shared/shared.module';
import { organisationFeatureKey, organisationReducer } from './store/organisation.reducer';
import { OrganisationEffects } from './store/organisation.effects';

import { OrganisationPickerComponent } from './components/organisation-picker/organisation-picker.component';
import { OrganisationEditorComponent } from './components/organisation-editor/organisation-editor.component';
import { OrganisationPopoverComponent } from './components/organisation-popover/organisation-popover.component';
import { MemberListComponent } from './components/member-list/member-list.component';



@NgModule({
  declarations: [
    OrganisationPickerComponent,
    OrganisationEditorComponent,
    OrganisationPopoverComponent,
    MemberListComponent
  ],
  imports: [
    SharedModule,

    StoreModule.forFeature(organisationFeatureKey, organisationReducer),
    EffectsModule.forFeature([OrganisationEffects]),
  ],
  exports: [
    OrganisationPickerComponent
  ]
})
export class OrganisationModule { }
