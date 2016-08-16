(function() {
  'use strict';

  angular.module('hale.gui')
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
})();
