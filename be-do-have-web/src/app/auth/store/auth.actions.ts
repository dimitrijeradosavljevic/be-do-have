import { createAction, props } from '@ngrx/store';
import { User } from '../../_shared/models/User';

export const authInit = createAction(
  '[Auth] User Init'
);

export const getCurrentUser = createAction(
  '[Auth] After user login or on init'
);

export const login = createAction(
  '[Login Page] User Login',
  props<{ user: { email: string, password: string } }>()
);

export const loginSuccess = createAction(
  '[Auth] User Login Success',
  props<{ user: User }>()
);

export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: any }>()
);

export const register = createAction(
  '[Register Page] User Register',
  props<{ user: { email: string, password: string, confirmedPassword: string } }>()
);

export const registerSuccess = createAction(
  '[Register] User Register Success',
  props<{ user: User }>()
);

export const registerFailure = createAction(
  '[Register] Register Failure',
  props<{ error: any }>()
);

export const logout = createAction(
  '[Auth] Logout'
);

export const logoutSuccess = createAction(
  '[Auth] Logout Success'
);

export const updateSuccess = createAction(
  '[Auth] User Edit Success',
  props<{ user: User }>()
);
