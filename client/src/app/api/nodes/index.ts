import 'rxjs/add/operator/map';

import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { environment as env } from 'environments/environment';


@Injectable()
export class Nodes {

  private baseUrl: string = `${env.apiUrl}/nodes`;
  private options: {[key: string]: any} = { withCredentials: true };

  constructor(private http: Http) {}

  list(): Observable<any> {
    return this.http
      .get(this.baseUrl, this.options)
      .map(res => res.json());
  }

  get(id: number): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/${id}`, this.options)
      .map(res => res.json());
  }

  update(host: any): Observable<any> {
    return this.http
      .post(`${this.baseUrl}/${host.id}`, host, this.options)
      .map(res => res.json());
  }
}
