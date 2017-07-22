import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { StateService } from 'ui-router-ng2';

import { Auth, ICredentials } from 'app/api/auth';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrls: [
    './login.scss'
  ]
})
export class Login {

  credentials: ICredentials;

  constructor(
    private auth: Auth,
    private stateService: StateService) {}

  doLogin(form: NgForm): void {
    this.auth.login(form.value)
      .subscribe(
        () => { this.stateService.transitionTo('app.hale.nodes') },
        (error: any) => { console.log(error)}
      );
  }
}
