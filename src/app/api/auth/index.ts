import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { environment as env }  from 'environments/environment';


@Injectable()
export default class Auth {
  private baseUrl: string = `${env.apiUrl}/authentication`;
  private options: {[key: string]: any} = { withCredentials: true };

  constructor(private http: Http) {}

  login(credentials: ICredentials): Observable<any> {
    return this.http
      .post(
        this.baseUrl,
        credentials,
        this.options,
      );
  }

  logout(): Observable<any> {
    return this.http
      .delete(this.baseUrl, this.options);
  }

  check(): Observable<any> {
    return this.http
      .get(this.baseUrl, this.options);
  }
}

export interface ICredentials {
  username: string;
  password: string;
  persistant: boolean;
}

