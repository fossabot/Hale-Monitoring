
import 'rxjs/add/operator/map';

import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { environment as env } from 'environments/environment';

@Injectable()
export class Results {

  private baseUrl: string = `${env.apiUrl}/nodes`;
  private options: {[key: string]: any} = { withCredentials: true };

  constructor(private http: Http) {}

  list(nodeId: number): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/${nodeId}/results`, this.options)
      .map(r => r.json());
  }
}
