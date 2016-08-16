(function() {
  'use strict';

  angular.module('hale.gui')
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
          redirectTo: '/settings/profile'
        })
        .when('/settings/:section', {
          templateUrl: './views/settings.html',
          controller: 'SettingsController'
        })
        .when('/admin', {
          redirectTo: '/admin/teams'
        })
        .when('/admin/:section', {
          templateUrl: './views/admin.html',
          controller: 'AdminController'
        })
        .otherwise({
          redirectTo: '/dashboard'
        });
    }]);
})();
