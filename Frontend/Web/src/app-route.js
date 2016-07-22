angular.module('HaleGUI')
  .config(['$routeProvider', function($routeProvider) {
    $routeProvider
      .when('/dashboard', {
        templateUrl: './views/main.html',
        controller: 'MainController'
      })
      .when('/settings/dashboard', {
        templateUrl: './views/settings/dashboard.html',
        controller: 'DashboardSettingsController'
      })
      .when('/settings/profile', {
        templateUrl: './views/settings/profile.html',
        controller: 'ProfileSettingsController'
      })
      .when('/settings/teams', {
        templateUrl: './views/settings/teams.html',
        controller: 'TeamsSettingsController'
      })
      .when('/settings/metadata', {
        templateUrl: './views/settings/metadata.html',
        controller: 'MetadataSettingsController'
      })
      .when('/login', {
        templateUrl: './views/login.html',
        controller: 'LoginController'
      })
      .when('/nodes', {
        templateUrl: './views/nodes.html',
        controller: 'NodesController'
      })
      .when('/settings', {
        redirectTo: '/settings/profile'
      })
      .otherwise({
        redirectTo: '/dashboard'
      });
  }]);
