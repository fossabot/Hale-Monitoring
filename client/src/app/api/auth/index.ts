import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { environment as env } from 'environments/environment';


@Injectable()
export class Auth {
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

  checkAdmin(): Observable<any> {
    return this.http
      .get(`${this.baseUrl}/admin`, this.options);
  }

  activate(attempt: IActivationAttempt): Observable<any> {
    return this.http
      .post(
        `${this.baseUrl}/activate`,
        attempt,
        this.options
      );
  }

  changePassword(oldPassword: string, newPassword: string): Observable<any> {
    return this.http
      .post(
        `${this.baseUrl}/change-password`,
        {
          oldPassword: oldPassword,
          newPassword: newPassword
        },
        this.options
      );
  }
}

export interface IActivationAttempt {
  username: string;
  activationPassword: string;
  newPassword: string;
}

export interface ICredentials {
  username: string;
  password: string;
  persistant: boolean;
}

