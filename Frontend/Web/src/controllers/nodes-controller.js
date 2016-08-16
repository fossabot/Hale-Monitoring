(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('NodesController', ['$scope', function($scope) {

          $scope.submenuItems = [
            {
              'label' : 'Summary',
              'url' : '/nodes/summary'
            },
            {
              'label' : 'Map',
              'url' : '/nodes/map'
            }
          ]
    }]);
})();
