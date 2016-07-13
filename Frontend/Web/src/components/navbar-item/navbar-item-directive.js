angular.module('HaleGUI')
  .directive('navitem', function() {
    return {
      templateUrl: './views/partials/navbar-item.html',
      controller: 'NavbarItemController',
      restriction: 'AE',
      scope: {
        'url': '@url',
        'icon': '@icon',
        'label': '@label'
      }
    };
  });
