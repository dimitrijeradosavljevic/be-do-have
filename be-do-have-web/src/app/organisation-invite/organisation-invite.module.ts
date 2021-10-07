import { NgModule } from '@angular/core';

import { SharedModule } from '../_shared/shared.module';

import { InviteListComponent } from './invite-list/invite-list.component';



@NgModule({
  declarations: [ InviteListComponent ],
  imports: [
    SharedModule
  ]
})
export class OrganisationInviteModule { }
