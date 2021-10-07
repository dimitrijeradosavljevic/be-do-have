import {Injectable} from '@angular/core';
import {ToastController} from '@ionic/angular';

import { catchError, map, mergeMap } from 'rxjs/operators';
import { of } from 'rxjs';

import { Action } from '@ngrx/store';
import { Actions, createEffect, ofType, OnInitEffects } from '@ngrx/effects';


import { OrganisationService } from '../services/organisation.service';
import * as OrganisationActions from './organisation.actions';


@Injectable()
export class OrganisationEffects implements OnInitEffects {

  // organisationInit$ = createEffect(() => this.actions$
  //   .pipe(
  //     ofType(OrganisationActions.organisationInit)
  //   )
  // );

  fetchOrganisations$ = createEffect(() => this.actions$
    .pipe(
      ofType(OrganisationActions.fetchOrganisations),
      mergeMap(() => this.organisationService.fetchUserOrganisations()
        .pipe(
          map(response => {
            return OrganisationActions.fetchOrganisationsSuccess({ data: response });
          }),
          catchError((err) => {
            console.log(err.message);
            // this.showErrorToast(err.error);
            return of(OrganisationActions.fetchOrganisationsFailure());
          })
        )
      )
    )
  );

  addOrganisation$ = createEffect(() => this.actions$
    .pipe(
      ofType(OrganisationActions.addOrganisation),
      mergeMap((action) => this.organisationService.store(action.organisation)
        .pipe(
          map(response => {
            this.showToast('Organisation added successfully.', 'success');
            action.closeModalEvent.emit();
            return OrganisationActions.addOrganisationSuccess({ organisation: response });
          }),
          catchError((err) => {
            action.closeModalEvent.emit();
            this.showToast(err.message, 'danger');
            return of(OrganisationActions.addOrganisationFailure());
          })
        )
      )
    )
  );

  editOrganisation$ = createEffect(() => this.actions$
    .pipe(
      ofType(OrganisationActions.editOrganisation),
      mergeMap((action) => this.organisationService.update(action.organisation)
        .pipe(
          map(response => {
            this.showToast('Organisation edited successfully.', 'success');
            action.closeModalEvent.emit();
            return OrganisationActions.editOrganisationSuccess({ organisation: response });
          }),
          catchError((err) => {
            action.closeModalEvent.emit();
            this.showToast(err.message, 'danger');
            return of(OrganisationActions.editOrganisationFailure());
          })
        )
      )
    )
  );



  constructor(
    private toastController: ToastController,
    private actions$: Actions,
    private organisationService: OrganisationService
    ) {

  }

  ngrxOnInitEffects(): Action {
    return OrganisationActions.organisationInit();
  }


  async showToast(message, color) {

    const toast = await this.toastController.create({
      message,
      duration:      2000,
      position:      'top',
      color,
      keyboardClose: true,
      animated:      true
    });
    toast.present();
  }
}
