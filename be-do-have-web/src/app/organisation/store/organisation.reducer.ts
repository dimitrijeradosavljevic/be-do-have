import { createReducer, on } from '@ngrx/store';

import * as OrganisationActions from './organisation.actions';
import { Organisation } from '../../_shared/models/Organisation';
import {selectedOrganisation} from "./organisation.selectors";

export const organisationFeatureKey = 'organisation';

export interface OrganisationState {
  organisations: Organisation[];
  selectedOrganisation: Organisation;
  isLoading: boolean;
}

export const initialState: OrganisationState = {
  organisations: [],
  selectedOrganisation: null,
  isLoading: false
};

export const organisationReducer = createReducer(
  initialState,
  on(OrganisationActions.organisationInit,
    (state) => ({
      ...state
    })
  ),

  on(OrganisationActions.fetchOrganisations,
    (state) => ({
      ...state,
      isLoading: true
    })
  ),

  on(OrganisationActions.fetchOrganisationsFailure,
    (state) => ({
      ...state,
      isLoading: false
    })
  ),

  on(OrganisationActions.fetchOrganisationsSuccess,
    (state, { data }) => ({
      ...state,
      organisations: data,
      isLoading: false
    })
  ),

  on(OrganisationActions.addOrganisation,
    (state) => ({
      ...state
    })
  ),

  on(OrganisationActions.addOrganisationSuccess,
    (state, { organisation }) => ({
      ...state,
      organisations: [...state.organisations, organisation],
      selectedOrganisation: organisation
    })
  ),

  on(OrganisationActions.becomeOrganisationMember,
    (state, { organisation }) => ({
      ...state,
      organisations: [...state.organisations, organisation]
    })
  ),

  on(OrganisationActions.editOrganisation,
    (state) => ({
      ...state
    })
  ),

  on(OrganisationActions.editOrganisationSuccess,
    (state, { organisation}) => ({
      ...state,
      organisations: [...state.organisations.map(o => {
        if (o.id === organisation.id) {
          return organisation;
        }
        return o;
      })],
      selectedOrganisation: organisation
    })
  ),

  on(OrganisationActions.selectOrganisation,
    (state, { organisation }) => ({
      ...state,
      selectedOrganisation: organisation
    }))
);
