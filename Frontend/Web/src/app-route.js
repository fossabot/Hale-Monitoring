(function() {
  'use strict';

  angular.module('hale.gui')
    .config([
      '$urlRouterProvider',
      '$stateProvider',
      function($urlRouterProvider, $stateProvider) {

        $urlRouterProvider.otherwise('/dashboard');
        $stateProvider
          .state('app', {
            abstract: true,
          })
          .state('app.login', {
            url: '/login',
            views: {
              'main@': {
                templateUrl: './views/login.html',
                controller: 'LoginController',
                controllerAs: '$ctrl'
              }
            }
          })
          .state('app.login.failed', {
            url: '/failed',
            views: {
              'toast@': {
                templateUrl: './views/toasts/login-failed.html',
                controller: 'ToastController',
                controllerAs: '$ctrl'
              }
            }
          })
          .state('app.hale', {
            abstract: true,
            resolve: {
              'authentication': (Auth) => {
                return Auth.authorize();
              },
            },
            views: {
              'nav@': {
                templateUrl: './views/partials/navbar.html',
                controller: 'NavbarController',
                controllerAs: '$ctrl'
              }
            }
          })
          .state('app.hale.dashboard', {
            url: '/dashboard',
            views: {
              'main@': {
                templateUrl: './views/dashboard.html',
                controller: 'DashboardController',
                controllerAs: '$ctrl'
              }
            }
          })
          .state('app.hale.nodes', {
            url: '/nodes',
            views: {
              'main@': {
                templateUrl: './views/nodes.html',
                controller: 'NodesController',
                controllerAs: '$ctrl',
                resolve: {
                  'nodes': ['Nodes', (Nodes) => {
                    return Nodes.list();
                  }]
                }
              }
            }
          })
          .state('app.hale.node', {
            url: '/node/:id',
            views: {
              'main@': {
                templateUrl: './views/node.html',
                controller: 'NodeController',
                controllerAs: '$ctrl',
                resolve: {
                  'node': ['$stateParams', 'Nodes', ($stateParams, Nodes) => {
                    return Nodes.get($stateParams.id);
                  }]
                }
              }
            }
          });

      /*$routeProvider
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
        .when('/nodes/:id', {
          templateUrl: './views/node.html',
          controller: 'NodeController'
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
        }); */
    }]);
})();
