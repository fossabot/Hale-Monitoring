(function() {
  'use strict';

  angular.module('HaleGUI')
    .directive('nodesMenu', function() {
      return {
        templateUrl: './views/partials/submenu.html',
        controller: 'NodesMenuController',
      }
    })
})();
