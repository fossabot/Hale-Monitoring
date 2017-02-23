import { IComponentOptions } from 'angular';

const template = require('./login-form.html');
require('./login-form.scss');

export class LoginFormComponent implements IComponentOptions {
  templateUrl: any;
  controller: any;

  constructor() {
    this.templateUrl = template;
    this.controller = [
      'Auth',
      LoginFormController
    ];
  }
}

export class LoginFormController {
  credentials: ICredentials;

  constructor(private Auth: any) {}

  doLogin(): void {
    this.Auth.login(this.credentials);
  }

}
  
interface ICredentials {
  username: string;
  password: string;
}