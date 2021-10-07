import { Component, OnInit } from '@angular/core';
import {select, Store} from '@ngrx/store';
import {State} from '../../../_shared/store';
import {PageSideMenuDto} from '../../../_shared/models/PageSideMenuDto';
import {selectedOrganisation} from '../../../organisation/store/organisation.selectors';
import {fetchPagesTree, fetchPagesTreeSuccess} from '../../store/page.actions';
import {pages} from '../../store/page.selectors';
import {OrganisationService} from "../../../organisation/services/organisation.service";

@Component({
  selector: 'app-pages-list-side-menu',
  templateUrl: './page-list-side-menu.component.html',
  styleUrls: ['./page-list-side-menu.component.scss'],
})
export class PageListSideMenuComponent implements OnInit {

  public pages: PageSideMenuDto[] = [];
  private selectedOrganisation;
  private selectedOrganisationSubscription$;
  private pagesTreeSubscription$;

  constructor(private store: Store<State>, private organisationService: OrganisationService) { }

  ngOnInit() {
    this.selectedOrganisationSubscription$ = this.store.pipe(select(selectedOrganisation))
      .subscribe(organisationData => {
        this.selectedOrganisation = organisationData;
        if (this.selectedOrganisation) {

          // this.organisationService.fetchDocumentTree(this.selectedOrganisation.id).subscribe(
          //   result => {
          //     this.pages = result.descedants;
          //     const copy = [... this.pages];
          //     this.store.dispatch(fetchPagesTreeSuccess({ pages: copy }));
          //   }
          // );

          this.store.dispatch(fetchPagesTree({ organisationId: this.selectedOrganisation.id }));
        }
      });

    this.pagesTreeSubscription$ = this.store.pipe(select(pages))
      .subscribe(pagesData => {
        this.pages = this.copyTree(pagesData);
      });
  }

  copyTree(descedants: PageSideMenuDto[]): PageSideMenuDto[] {
    return descedants.map(p => {
      return {
        id: p.id,
        title: p.title,
        descedants: this.copyTree(p.descedants),
        iconName: p.iconName,
        iconColor: p.iconColor
      } as PageSideMenuDto;
    });
  }

}
