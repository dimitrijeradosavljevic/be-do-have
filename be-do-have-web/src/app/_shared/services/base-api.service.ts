import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import {throwError} from "rxjs";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class BaseApiService {

  public apiUrl: string;

  constructor(protected http: HttpClient,
              protected router: Router) {

    this.apiUrl = environment.baseUrl;
  }

  public handleError(response: any) {
    if (response.status === 401) {
      this.handleAuthError(response);
    }

    return throwError(response);
  }

  public handleAuthError(response) {
    this.router.navigate(['/login']);
  }
}
