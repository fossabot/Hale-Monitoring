angular.module('hale.gui', [
    'ui.router',
    'ui.gravatar',
    'angular-storage',
    'toastr',
    'ngTable',
    require('./api/api.js'),
    require('./login/login.js'),
    require('./shared/shared.js'),
    require('./nodes/nodes.js')
]);

require('./app.routes.js');
require('./app.config.js');


