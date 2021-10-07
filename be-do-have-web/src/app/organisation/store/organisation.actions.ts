import { EventEmitter } from '@angular/core';
import { createAction, props } from '@ngrx/store';

import { Organisation } from '../../_shared/models/Organisation';


export const organisationInit = createAction(
  '[Organisation] Init',
);

export const fetchOrganisations = createAction(
  '[Organisation] Fetch Organisations'
);

export const fetchOrganisationsFailure = createAction(
  '[Organisation] Fetch Organisations Failure'
);

export const fetchOrganisationsSuccess = createAction(
  '[Organisation] Fetch Organisations Success',
  props<{ data: Organisation[] }>()
);

export const addOrganisation = createAction(
  '[Organisation] Add Organisation',
  props<{ organisation: Organisation; closeModalEvent: EventEmitter<any> }>()
);

export const addOrganisationSuccess = createAction(
  '[Organisation] Add Organisation Success',
  props<{organisation: Organisation }>()
);

export const addOrganisationFailure = createAction(
  '[Organisation] Add Organisation Failure'
);

export const becomeOrganisationMember = createAction(
  '[Organisation] Become Organisation Member',
  props<{organisation: Organisation }>()
);

export const editOrganisation = createAction(
  '[Organisation] Edit Organisation',
  props<{ organisation: Organisation; closeModalEvent: EventEmitter<any> }>()
);

export const editOrganisationSuccess = createAction(
  '[Organisation] Edit Organisation Success',
  props<{organisation: Organisation}>()
);

export const editOrganisationFailure = createAction(
  '[Organisation] Edit Organisation Failure'
);

export const selectOrganisation = createAction(
  '[Organisation] Select Organisation',
  props<{organisation: Organisation}>()
);

