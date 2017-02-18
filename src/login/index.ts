import * as angular from 'angular';

import { LoginComponent } from './login';
import { LoginFormComponent } from './login-form';

export default angular
  .module('hale.login', [])
  .component('login', new LoginComponent())
  .component('loginForm', new LoginFormComponent()) 
  .name;