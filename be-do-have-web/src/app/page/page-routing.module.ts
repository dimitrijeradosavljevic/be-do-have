import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PageDetailComponent } from './components/page-detail/page-detail.component';


const routes: Routes = [
  {
    path: 'pages/:pageId',
    component: PageDetailComponent
  },
  {
    path: 'pages',
    component: PageDetailComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PageRoutingModule { }
