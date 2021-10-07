import { Injectable } from '@angular/core';
import { HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { BaseApiService } from '../../_shared/services/base-api.service';

import { Page } from '../../_shared/models/Page';
import { PageSideMenuDto } from '../../_shared/models/PageSideMenuDto';
import { PageSearchParameters } from '../../_shared/models/PageSearchParameters';
import * as moment from 'moment';
import {PageSearch} from "../../_shared/models/PageSearch";
import {PaginationResponse} from "../../_shared/models/PaginationResponse";
import {Organisation} from "../../_shared/models/Organisation";


@Injectable({
  providedIn: 'root'
})
export class PageService extends BaseApiService {

  public pagesForPicker(title: string, pageIndex: number, pageSize: number): Observable<PaginationResponse<Page>> {
    let params = new HttpParams();

    params = (title ? params.set('keyword', title) : params);
    params = (pageIndex ? params.set('pageIndex', pageIndex) : params);
    params = (pageSize ? params.set('pageSize', pageSize) : params);

    return this.http.get<PaginationResponse<Page>>(this.apiUrl + '/api/page/for-picker', { params })
      .pipe(catchError(response => this.handleError(response)));
  }

  public show(pageId: number): Observable<Page> {

    return this.http.get<Page>(this.apiUrl + `/api/page/${pageId}`)
      .pipe(catchError(response => this.handleError(response)));
  }

  public store(page: Page): Observable<Page> {

    return this.http.post<Page>(this.apiUrl + '/api/page', page)
      .pipe(catchError(response => this.handleError(response)));
  }

  public update(page: Page): Observable<Page> {

    return this.http.put<Page>(this.apiUrl + `/api/page/${page.id}`, page)
      .pipe(catchError(response => this.handleError(response)));
  }

  public destroy(pageId: number): Observable<any> {

    return this.http.delete(this.apiUrl + `/api/page/${pageId}`)
      .pipe(catchError(response => this.handleError(response)));
  }

  public trash(pageId: number, trash: boolean): Observable<any> {

    const page = new Page();
    page.id = pageId;
    page.archived = trash;

    return this.http.put(this.apiUrl + `/api/page/${pageId}/trash`, page)
      .pipe(catchError(response => this.handleError(response)));
  }

  public trashed(title: string, organisationId: number, pageIndex: number, pageSize: number, orderBy: string, direction: string = 'DESC'): Observable<PaginationResponse<Page>> {
    let params = new HttpParams();

    params = (title ? params.set('keyword', title) : params);
    params = (organisationId ? params.set('organisationId', JSON.stringify(organisationId)) : params);
    params = (pageIndex ? params.set('pageIndex', JSON.stringify(pageIndex)) : params);
    params = (pageSize ? params.set('pageSize', JSON.stringify(pageSize)) : params);
    params = (orderBy ? params.set('orderBy', orderBy) : params);
    params = (direction ? params.set('direction', direction) : params);

    return this.http.get<PaginationResponse<Page>>(this.apiUrl + '/api/page/trashed', { params })
      .pipe(catchError(response => this.handleError(response)));
  }

  public copyTree(descedants: PageSideMenuDto[]): PageSideMenuDto[] {
    return descedants.map(p => {
      console.log(p.open);
      return {
        id: p.id,
        title: p.title,
        open: p.open,
        descedants: this.copyTree(p.descedants)
      } as PageSideMenuDto;
    });
  }

  public insertNode(nodes: PageSideMenuDto[], directPageId: number, newNode: PageSideMenuDto): PageSideMenuDto[] {
    if (directPageId == null) {
      return [...nodes, newNode];
    }
    return nodes.map(page => {
      if (page.id == directPageId) {
        page.descedants = [...page.descedants, newNode];
      }
      else {
        page.descedants = this.insertNode(page.descedants, directPageId, newNode);
      }
      return page;
    });
  }

  public deleteNode(nodes: PageSideMenuDto[], pageId: number): PageSideMenuDto[] {
    return nodes.filter(page => {
      if (page.id === pageId) {
        return false;
      }
      else {
        page.descedants = this.deleteNode(page.descedants, pageId);
      }
      return true;
    });
  }

  public search(parameters: PageSearchParameters): Observable<PageSearch[]> {
    return this.http.post<PageSearch[]>(this.apiUrl + `/api/page/search`, parameters)
      .pipe(catchError(response => this.handleError(response)));
  }

  public searchPage(parameters: PageSearchParameters): Observable<string> {
    return this.http.post<string>(this.apiUrl + `/api/page/${parameters.pageId}/search`, parameters)
      .pipe(catchError(response => this.handleError(response)));
  }

  public autoCompleteSuggest(term: string): Observable<string[]> {
    let params = new HttpParams();

    params = (term ? params.set('term', term) : params);

    return this.http.get<string[]>(this.apiUrl + '/api/page/auto-complete', { params })
      .pipe(catchError(response => this.handleError(response)));
  }

  public movePage(pageId: number, directParentId: number) {
    return this.http.put(this.apiUrl + `/api/page/${pageId}/move`, directParentId)
      .pipe(catchError(response => this.handleError(response)));
  }

  public movePageUnderOrganisation(pageId: number, organisationId: number): Observable<Organisation> {
    return this.http.put<Organisation>(this.apiUrl + `/api/page/${pageId}/move-under-organisation`, organisationId)
      .pipe(catchError(response => this.handleError(response)));
  }

}
