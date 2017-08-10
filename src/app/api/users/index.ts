import 'rxjs/add/operator/map';

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http } from '@angular/http';
import { Md5 } from 'ts-md5/dist/md5';

import { environment as env } from 'environments/environment';

@Injectable()
export class Users {

  private baseUrl: string = `${env.apiUrl}/users`;
  private options: {[key: string]: any} = { withCredentials: true};

  constructor(private http: Http) {}

  list(): Observable<any> {
    return this.http
      .get(this.baseUrl, this.options)
      .map(r => r.json());
  }

  create(newUser: any) {
    return this.http
      .post(this.baseUrl, newUser, this.options);
  }

  getById(id: number): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/${id}`, this.options)
      .map(r => r.json());
  }

  getCurrent(): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/current`, this.options)
      .map(r => r.json());
  }

  getGravatarUrl(email: string): string {
    const hash: string = Md5.hashStr(email).toString();
    return `https://www.gravatar.com/avatar/${hash}`;
  }

  updateUser(userData: IUpdateUser) {
    return this.http
      .patch(`${this.baseUrl}/${userData.id}`, userData, this.options);
  }

  checkIfAvailable(username: string): Observable<any> {
    console.log('HEEERE');

    return this.http
      .get(`${this.baseUrl}/available?username=${encodeURIComponent(username)}`, this.options)
      .map(r => r.json());
  }
}

export interface IUpdateUser {
  id: number;
  email: string;
  fullName: string;
  activated: boolean;
  enabled: boolean;
}
