import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AuthState, authFeatureKey } from './auth.reducer';


export const selectAuthState = createFeatureSelector<AuthState>(authFeatureKey);


export const isLoggedIn = createSelector(
  selectAuthState,
  (state: AuthState) => state.userAuthCheck ? undefined : !!state.user
);

export const authUser = createSelector(
  selectAuthState,
  (state: AuthState) => state.user
);

export const isAuthUserFilled = createSelector(
  selectAuthState,
  (state: AuthState) => !(!state.user.fullName || !state.user.phoneNumber)
);

export const isNotLoggedIn = createSelector(
  selectAuthState,
  (state: AuthState) => state.userAuthCheck ? undefined : !state.user
);

export const isCheckingAuthUser = createSelector(
  selectAuthState,
  (state: AuthState) => state.userAuthCheck
);

