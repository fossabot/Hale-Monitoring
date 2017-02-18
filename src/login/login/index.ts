import { IComponentOptions } from 'angular';

const template = require('./login.html');

export class LoginComponent implements IComponentOptions {
  templateUrl: any;

  constructor() {
    this.templateUrl = template; 
  }
}
