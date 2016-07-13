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
      .otherwise({
        redirectTo: '/dashboard'
      });
  }]);
