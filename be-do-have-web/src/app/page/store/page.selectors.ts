import { createFeatureSelector, createSelector } from '@ngrx/store';
import { PageState, pageFeatureKey } from './page.reducer';

export const selectPageState = createFeatureSelector<PageState>(pageFeatureKey);

export const pages = createSelector(
  selectPageState,
  state => state.pages
);

export const isLoading = createSelector(
  selectPageState,
  state => state.isLoading
);
