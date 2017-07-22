import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http } from '@angular/http';
import { Md5 } from 'ts-md5/dist/md5';

import { environment as env }  from 'environments/environment';

@Injectable()
export class Users {

  private baseUrl: string = `${env.apiUrl}/users`;
  private options: {[key: string]: any} = { withCredentials: true};

  constructor(private http: Http) {}

  getCurrent(): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/current`, this.options)
      .map(r => r.json());
  }

  getGravatarUrl(email: string): string {
    const hash: string = Md5.hashStr(email).toString();
    return `https://www.gravatar.com/avatar/${hash}`;
  }
}
