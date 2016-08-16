(function() {
  'use strict';
  angular.module('hale.gui')
    .controller('HostSummaryWidgetController', ['$scope', 'Nodes', 'NodeConstants', function($scope, Nodes, NodeConstants) {
      $scope.status = NodeConstants.StatusBg;

      $scope.onNodesListed = function(response) {
        $scope.hosts = response;
      }
      Nodes.List($scope.onNodesListed);

      $scope.limit = 10;
      $scope.offset = 0;

      $scope.increaseOffset = function() {
        if ($scope.offset + $scope.limit < $scope.hosts.length) {
          console.log('Increasing');
          $scope.offset += $scope.limit;
        }

      }
      $scope.decreaseOffset = function() {
        if ($scope.offset > 0) {
          console.log('Decreasing');
          $scope.offset -= $scope.limit;
        }
      }
  }]);
})();
