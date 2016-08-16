(function() {
  'use strict';

  angular.module('hale.gui')
    .directive('nodesMenu', function() {
      return {
        templateUrl: './views/partials/submenu.html',
        controller: 'NodesMenuController',
      }
    })
})();
