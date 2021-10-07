import { createAction, props } from '@ngrx/store';

import { PageSideMenuDto } from '../../_shared/models/PageSideMenuDto';


export const pageInit = createAction(
  '[Page] Init',
);

export const fetchPagesTree = createAction(
  '[Page] Fetch Pages Tree',
  props<{ organisationId: number }>()
);

export const fetchPagesTreeSuccess = createAction(
  '[Page] Fetch Pages Tree Success',
  props<{ pages: PageSideMenuDto[] }>()
);

export const fetchPagesTreeFailure = createAction(
  '[Page] Fetch Pages Tree Failure'
);

export const addPageInTree = createAction(
  '[Page] Add Page In Tree',
  props<{ page: PageSideMenuDto; directParentId?: number }>()
);
