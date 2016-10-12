require('angular');
require('angular-ui-router');
require('angular-storage');
require('angular-gravatar');
require('angular-drag-and-drop-lists');

require('bootstrap/dist/css/bootstrap.css');
require('font-awesome/css/font-awesome.css');


require('./sass/app.scss');

angular.module('hale.gui', [
    'ui.router',
    'ui.gravatar',
    'angular-storage',
    'dndLists',
]);

require('./app-route.js');
require('./app-config.js');
require('./services/services.js');
require('./controllers/controllers.js');
require('./components/components.js');
require('./directives/directives.js');


