import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { environment as env } from 'environments/environment';

@Injectable()
export class Configs {
  private baseUrl: string = `${env.apiUrl}/configs`;
  private options: {[key: string]: any} = { withCredentials: true };

  constructor(private http: Http) {}

  list(): Observable<any> {
    return this.http
      .get(
        this.baseUrl,
        this.options
      )
      .map(r => r.json());
  }

  get(id: number): Observable<any> {
    return this.http
      .get(
        `${this.baseUrl}/${id}`,
        this.options,
      )
      .map(r => r.json());
  }

  save(id: number, config: string): Observable<any> {
    return this.http
      .post(
        `${this.baseUrl}/${id}`,
        { body: config },
        this.options
      );
  }
}
