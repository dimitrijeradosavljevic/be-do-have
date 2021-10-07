import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { reducers } from './_shared/store';

import { SharedModule } from './_shared/shared.module';
import { AuthModule } from './auth/auth.module';
import { PageModule } from './page/page.module';
import { OrganisationModule } from './organisation/organisation.module';
import { OrganisationInviteModule } from './organisation-invite/organisation-invite.module';
import {TagModule} from "./tag/tag.module";


@NgModule({
  declarations: [ AppComponent ],
  entryComponents: [],
  imports: [
    BrowserModule,
    IonicModule.forRoot(),
    AppRoutingModule,

    StoreModule.forRoot(reducers),
    EffectsModule.forRoot([]),

    SharedModule,
    AuthModule,
    PageModule,
    OrganisationModule,
    OrganisationInviteModule,
    TagModule
  ],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy } ],
  bootstrap: [AppComponent],
})
export class AppModule {}
