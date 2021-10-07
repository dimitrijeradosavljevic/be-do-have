import {Injectable} from '@angular/core';
import {Router} from '@angular/router';

import {catchError, map, mergeMap, withLatestFrom} from 'rxjs/operators';
import {of} from 'rxjs';

import {Action, Store} from '@ngrx/store';
import {Actions, createEffect, ofType, OnInitEffects} from '@ngrx/effects';

import * as PageActions from './page.actions';

import {OrganisationService} from '../../organisation/services/organisation.service';
import {State} from "../../_shared/store";
import {pages} from "./page.selectors";
import {PageService} from "../services/page.service";


@Injectable()
export class PageEffects implements OnInitEffects {

  // pageInit$ = createEffect(() => this.actions$
  //   .pipe(
  //     ofType(PageActions.pageInit)
  //   )
  // );

  fetchPagesTree$ = createEffect(() => this.actions$
    .pipe(
      ofType(PageActions.fetchPagesTree),
      mergeMap((action) => this.organisationService.fetchDocumentTree(action.organisationId)
        .pipe(
          map(response => {
            return PageActions.fetchPagesTreeSuccess({ pages: response.descedants });
          }),
          catchError((err) => {
            return of(PageActions.fetchPagesTreeFailure());
          })
        )
      )
    )
  );

  addPageInTree$ = createEffect(() => this.actions$
    .pipe(
      ofType(PageActions.addPageInTree),
      withLatestFrom(this.store.select(pages)),
      map(([action, pagesFromState]) => {
        const sideMenuPages = this.pageService.copyTree(pagesFromState);

        const insertedNode = this.pageService.insertNode(sideMenuPages, action.directParentId, action.page);
        return PageActions.fetchPagesTreeSuccess({ pages: insertedNode });
      }
      )
    )
  );

  constructor(
    private store: Store<State>,
    private actions$: Actions,
    private router: Router,
    private pageService: PageService,
    private organisationService: OrganisationService) { }

  ngrxOnInitEffects(): Action {
    return PageActions.pageInit();
  }
}
