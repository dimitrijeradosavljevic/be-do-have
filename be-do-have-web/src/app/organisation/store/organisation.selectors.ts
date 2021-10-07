import { createFeatureSelector, createSelector } from '@ngrx/store';
import { OrganisationState, organisationFeatureKey } from './organisation.reducer';

export const selectOrganisationState = createFeatureSelector<OrganisationState>(organisationFeatureKey);

export const organisations = createSelector(
  selectOrganisationState,
  state => state.organisations
);

export const selectedOrganisation = createSelector(
  selectOrganisationState,
  state => state.selectedOrganisation
);

export const isLoading = createSelector(
  selectOrganisationState,
  state => state.isLoading
);
