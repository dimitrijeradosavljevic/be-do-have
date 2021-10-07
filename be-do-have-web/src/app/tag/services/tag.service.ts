import { Injectable } from '@angular/core';
import { HttpParams} from '@angular/common/http';

import { BaseApiService} from '../../_shared/services/base-api.service';

import { Observable} from 'rxjs';

import { Tag} from '../../_shared/models/Tag';
import { PaginationResponse} from '../../_shared/models/PaginationResponse';

@Injectable({
  providedIn: 'root'
})
export class TagService extends BaseApiService {

  public getTags(name: string, pageIndex: number, pageSize: number, orderBy: string = 'id', direction: string = 'DESC'): Observable<PaginationResponse<Tag>> {
    let params = new HttpParams();

    params = (name ? params.set('keyword', name) : params);
    params = (pageIndex ? params.set('pageIndex', JSON.stringify(pageIndex)) : params);
    params = (pageSize ? params.set('pageSize', JSON.stringify(pageSize)) : params);
    params = (orderBy ? params.set('orderBy', orderBy) : params);
    params = (direction ? params.set('direction', direction) : params);

    return this.http.get<PaginationResponse<Tag>>(this.apiUrl + '/api/tags', { params });
  }

  public store(tag: Tag): Observable<Tag> {
    return this.http.post<Tag>(this.apiUrl + '/api/tags', tag);
  }
}
