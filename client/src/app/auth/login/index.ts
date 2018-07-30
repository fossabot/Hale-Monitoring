import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { StateService } from '@uirouter/angular';

import { Auth, ICredentials } from 'app/api/auth';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrls: [
    './login.scss'
  ]
})
export class LoginComponent {

  credentials: ICredentials;

  constructor(
    private auth: Auth,
    private stateService: StateService,
    private notifications: NotificationsService) {}

  doLogin(form: NgForm): void {
    this.auth
      .login(form.value)
      .subscribe(
        () => {
          this.stateService.transitionTo('app.hale.nodes');
        },
        (error: any) => {
          this.notifications.error('Login failed', 'Check your credentials.');
        }
      );
  }
}
