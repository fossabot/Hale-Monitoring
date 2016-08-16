(function() {
  'use strict';

  angular.module('hale.gui')
    .directive('nodesList', function() {
      return {
        templateUrl: './views/lists/nodes.html',
        controller: 'NodesListController'
      }
    })
})();
