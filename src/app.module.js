import angular from 'angular';

import apiModule from './api';
import loginModule from './login';
import nodesModule from './nodes';
import commonModule from './common';

angular.module('hale.gui', [
    'ui.router',
    'ui.gravatar',
    'angular-storage',
    'toastr',
    'ngTable',
    apiModule,
    loginModule,
    nodesModule,
    commonModule,
]);

require('./app.routes.js');
require('./app.config.js');


