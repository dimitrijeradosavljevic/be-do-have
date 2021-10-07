import { NgModule } from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { CommonModule } from '@angular/common';

import { IonicModule } from '@ionic/angular';

import { HeaderComponent } from './components/header/header.component';
import { DatePickerRangeComponent } from './components/date-picker-range/date-picker-range.component';

import { ReactiveFormsModule } from '@angular/forms';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';
import { IconPickerComponent} from './components/icon-picker/icon-picker.component';


@NgModule({
  declarations: [ HeaderComponent, DatePickerRangeComponent, IconPickerComponent ],
  imports: [
    CommonModule,
    IonicModule,
    HttpClientModule,
    ReactiveFormsModule,


    AutocompleteLibModule,
  ],
  providers:    [
    {
      provide:  HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi:    true
    }
  ],
  exports: [
    CommonModule,
    IonicModule,
    ReactiveFormsModule,
    HttpClientModule,


    AutocompleteLibModule,

    HeaderComponent,
    DatePickerRangeComponent,
    IconPickerComponent,
  ]
})
export class SharedModule { }
