import { Injectable } from '@angular/core';
import { HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { BaseApiService } from '../../_shared/services/base-api.service';

import { PageSideMenuDto } from '../../_shared/models/PageSideMenuDto';
import { PaginationResponse } from '../../_shared/models/PaginationResponse';
import { Organisation } from '../../_shared/models/Organisation';
import { User } from '../../_shared/models/User';

@Injectable({
  providedIn: 'root'
})
export class OrganisationService extends BaseApiService {


  public index(filter: string, pageIndex: number, pageSize: number, orderBy: string = 'ID', direction: string = 'DESC'): Observable<PaginationResponse<Organisation>> {
    let params = new HttpParams();

    params = (filter ? params.set('keyword', filter) : params);
    params = (pageIndex ? params.set('pageIndex', JSON.stringify(pageIndex)) : params);
    params = (pageSize ? params.set('pageSize', JSON.stringify(pageSize)) : params);
    params = (orderBy ? params.set('orderBy', JSON.stringify(orderBy)) : params);
    params = (direction ? params.set('direction', JSON.stringify(direction)) : params);

    return this.http.get<PaginationResponse<Organisation>>(this.apiUrl + '/api/organisation', { params })
      .pipe(catchError(response => this.handleError(response)));
  }

  public show(organisationId: number): Observable<Organisation> {
    return this.http.get<Organisation>(this.apiUrl + `/api/organisation/${organisationId}`)
      .pipe(catchError(response => this.handleError(response)));
  }

  public fetchUserOrganisations(): Observable<Organisation[]> {
    return this.http.get<Organisation[]>(this.apiUrl + '/api/organisation/all')
      .pipe(catchError(response => this.handleError(response)));
  }

  public store(organisation: Organisation): Observable<Organisation> {
    return this.http.post<Organisation>(this.apiUrl + '/api/organisation', organisation)
      .pipe(catchError(response => this.handleError(response)));
  }

  public update(organisation: Organisation): Observable<Organisation> {
    return this.http.put<Organisation>(this.apiUrl + `/api/organisation/${organisation.id}`, organisation)
      .pipe(catchError(response => this.handleError(response)));
  }

  public fetchDocumentTree(organisationId: number): Observable<PageSideMenuDto> {

    return this.http.get<PageSideMenuDto>(this.apiUrl + `/api/organisation/${organisationId}/document-tree`)
      .pipe(catchError(response => this.handleError(response)));
  }

  public fetchNonMembers(organisationId: number, userName: string, pageIndex: number, pageSize: number, orderBy: string = 'id', direction: string = 'DESC'): Observable<PaginationResponse<User>> {
      let params = new HttpParams();

      params = (userName ? params.set('keyword', userName) : params);
      params = (pageIndex ? params.set('pageIndex', JSON.stringify(pageIndex)) : params);
      params = (pageSize ? params.set('pageSize', JSON.stringify(pageSize)) : params);
      params = (orderBy ? params.set('orderBy', orderBy) : params);
      params = (direction ? params.set('direction', direction) : params);

      return this.http.get<PaginationResponse<User>>(this.apiUrl + `/api/organisation/${organisationId}/non-members`, { params })
        .pipe(catchError(response => this.handleError(response)));
  }

  public fetchMembers(organisationId: number, pageIndex: number, pageSize: number, orderBy: string = 'id', direction: string = 'DESC'): Observable<PaginationResponse<User>> {
    let params = new HttpParams();

    params = (pageIndex ? params.set('pageIndex', JSON.stringify(pageIndex)) : params);
    params = (pageSize ? params.set('pageSize', JSON.stringify(pageSize)) : params);
    params = (orderBy ? params.set('orderBy', orderBy) : params);
    params = (direction ? params.set('direction', direction) : params);

    return this.http.get<PaginationResponse<User>>(this.apiUrl + `/api/organisation/${organisationId}/members`, { params })
      .pipe(catchError(response => this.handleError(response)));
  }

  public addMember(organisationId: number, userId: number) {
    return this.http.post(this.apiUrl + `/api/organisation/${organisationId}/add-member`, userId)
      .pipe(catchError(response => this.handleError(response)));
  }
}
