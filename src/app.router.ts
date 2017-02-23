export default ['$urlRouterProvider', '$stateProvider', AppRouter];

function AppRouter($urlRouterProvider, $stateProvider) {

  $urlRouterProvider.otherwise('/nodes/monitored');
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
      onEnter: function (Auth) {
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
      abstract: true
    })
    .state('app.hale.nodes.unconfigured', {
      url: '/nodes/unconfigured',
      views: {
        'main@': {
          template: '<unconfigured-nodes-list></unconfigured-nodes-list>'
        }
      }
    })
    .state('app.hale.nodes.monitored', {
      url: '/nodes/monitored',
      views: {
        'main@': {
          template: '<monitored-nodes-list></monitored-nodes-list>'
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