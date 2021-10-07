import { NgModule } from '@angular/core';

import { SharedModule } from '../_shared/shared.module';
import { TagsPickerComponent } from './components/tags-picker/tags-picker.component';




@NgModule({
  declarations: [TagsPickerComponent],
  exports: [
    TagsPickerComponent
  ],
  imports: [
    SharedModule
  ]
})

export class TagModule { }
