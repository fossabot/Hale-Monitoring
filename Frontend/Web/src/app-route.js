angular.module('HaleGUI')
  .config(['$routeProvider', function($routeProvider) {
    $routeProvider
      .when('/dashboard', {
        templateUrl: './views/main.html',
        controller: 'MainController'
      })
      .when('/login', {
        templateUrl: './views/login.html',
        controller: 'LoginController'
      })
      .when('/nodes', {
        templateUrl: './views/nodes.html',
        controller: 'NodesController'
      })
      .when('/reports', {
        templateUrl: './views/reports.html',
        controller: 'ReportsController'
      })
      .when('/services', {
        templateUrl: './views/services.html',
        controller: 'ServicesController'
      })
      .when('/settings', {
        templateUrl: './views/settings.html',
        controller: 'SettingsController'
      })
      .when('/settings/:section', {
        templateUrl: './views/settings.html',
        controller: 'SettingsController'
      })
      .otherwise({
        redirectTo: '/dashboard'
      });
  }]);
