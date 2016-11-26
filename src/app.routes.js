angular.module('hale.gui')
  .config(AppRoutes);

AppRoutes.$inject = [ '$urlRouterProvider', '$stateProvider' ];
function AppRoutes($urlRouterProvider, $stateProvider) {

  $urlRouterProvider.otherwise('/nodes');
  $stateProvider
    .state('app', {
      abstract: true,
    })
    .state('app.login', {
      url: '/login',
      views: {
        'main@': {
          template: '<login></login>',
        }
      }
    })
    .state('app.logout', {
      onEnter: function(Auth) {
        Auth.logout();
      }
    })
    .state('app.hale', {
      abstract: true,
      resolve: {
        'authentication': (Auth) => {
          return Auth.authenticate();
        },
      },
      views: {
        'nav@': {
          template: '<navbar></navbar>'
        }
      }
    })
    .state('app.hale.nodes', {
      url: '/nodes',
      views: {
        'main@': {
          template: '<nodes-list></nodes-list>'
        }
      }
    })
    .state('app.hale.node', {
      url: '/node/:id',
      views: {
        'main@': {
          template: '<node></node>'
        }
      }
    });
}