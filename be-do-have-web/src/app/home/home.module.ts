import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';
import { HomePage } from './home.page';

import { HomePageRoutingModule } from './home-routing.module';
import {SharedModule} from "../_shared/shared.module";
import {HttpClient, HttpClientModule} from "@angular/common/http";


@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        IonicModule,
        HomePageRoutingModule,
        SharedModule
    ],
  declarations: [HomePage]
})
export class HomePageModule {}
