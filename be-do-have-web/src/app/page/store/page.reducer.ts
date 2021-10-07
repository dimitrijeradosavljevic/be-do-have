import { createReducer, on } from '@ngrx/store';

import * as PageActions from './page.actions';
import { PageSideMenuDto } from '../../_shared/models/PageSideMenuDto';

export const pageFeatureKey = 'page';

export interface PageState {
  pages: PageSideMenuDto[];
  isLoading: boolean;
}

export const initialState: PageState = {
  pages: [],
  isLoading: false
};

export const pageReducer = createReducer(
  initialState,
  on(PageActions.pageInit,
    (state) => ({
      ...state
    })
  ),

  on(PageActions.fetchPagesTree,
    (state) => ({
      ...state,
      isLoading: true
    })
  ),

  on(PageActions.fetchPagesTreeSuccess,
    (state, { pages }) => ({
      ...state,
      pages,
      isLoading: false
    })
  ),

  // on(PageActions.addPageInTree,
  //   (state, { page, directParentId }) => ({
  //     ...state,
  //     pages: this.,
  //     isLoading: false
  //   })
  // ),

);
