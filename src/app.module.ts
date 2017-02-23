import * as angular from 'angular';

import appConfig from './app.config';
import appRouter from './app.router';

import apiModule from './api';
import loginModule from './login';
import nodesModule from './nodes';
import commonModule from './common';

require('./sass/app.scss');

angular
  .module('hale.gui', [
    'ui.router',
    'ui.gravatar',
    'angular-storage',
    'toastr',
    'ngTable',
    apiModule,
    loginModule,
    nodesModule,
    commonModule,
  ])
  .config(appConfig)
  .config(appRouter);