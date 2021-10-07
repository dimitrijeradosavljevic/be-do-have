import { Injectable } from '@angular/core';
import {catchError} from "rxjs/operators";
import {BaseApiService} from "../../_shared/services/base-api.service";
import {Observable} from "rxjs";
import {User} from "../../_shared/models/User";
import {HttpParams} from "@angular/common/http";
import {PaginationResponse} from "../../_shared/models/PaginationResponse";
import {OrganisationInvite} from "../../_shared/models/OrganisationInvite";
import {Organisation} from "../../_shared/models/Organisation";

@Injectable({
  providedIn: 'root'
})
export class OrganisationInviteService extends BaseApiService {

  invite(organisationId: number, currentUserId: number, userId: number) {
    const body = {
      inviterId: currentUserId,
      invitedId: userId
    };

    return this.http.post(this.apiUrl + `/api/organisation-invites/${organisationId}/invite`, body)
      .pipe(catchError(response => this.handleError(response)));
  }

  public getInvites(userId: number, pageIndex: number, pageSize: number, orderBy: string = 'id', direction: string = 'DESC'): Observable<PaginationResponse<OrganisationInvite>> {
    let params = new HttpParams();

    params = (userId ? params.set('userId', JSON.stringify(userId)) : params);
    params = (pageIndex ? params.set('pageIndex', JSON.stringify(pageIndex)) : params);
    params = (pageSize ? params.set('pageSize', JSON.stringify(pageSize)) : params);
    params = (orderBy ? params.set('orderBy', orderBy) : params);
    params = (direction ? params.set('direction', direction) : params);

    return this.http.get<PaginationResponse<OrganisationInvite>>(this.apiUrl + `/api/organisation-invites`, { params })
      .pipe(catchError(response => this.handleError(response)));
  }

  public storeResponse(id: number, userResponse: boolean): Observable<Organisation> {
    return this.http.post<Organisation>(this.apiUrl + `/api/organisation-invites/${id}`, userResponse)
      .pipe(catchError(response => this.handleError(response)));
  }

}
