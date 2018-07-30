import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http } from '@angular/http';

import { environment as env } from 'environments/environment';


@Injectable()
export class Comments {
  private baseUrl: string = `${env.apiUrl}/nodes`;
  options: {[key: string]: any} = { withCredentials: true };
  constructor(private http: Http) {}

  get(nodeId: number): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/${nodeId}/comments`, this.options)
      .map(r => r.json());
  }

  save(nodeId: number, text: string): Observable<any> {
    return this.http
      .post(
        `${this.baseUrl}/${nodeId}/comments`,
        { text: text},
        this.options
      );
  }

  delete(nodeId: number, commentId: number): Observable<any> {
    return this.http
      .delete(
        `${this.baseUrl}/${nodeId}/comments/${commentId}`,
        this.options
      );
  }
}
