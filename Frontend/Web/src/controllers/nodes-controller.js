(function() {
  'use strict';

  angular.module('HaleGUI')
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
