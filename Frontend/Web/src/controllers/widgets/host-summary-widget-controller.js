angular.module('HaleGUI')
  .controller('HostSummaryWidgetController', ['$scope', 'Nodes' , function($scope, Nodes) {
    $scope.nodes = Nodes.List();
    $scope.limit = 10;
    $scope.offset = 0;

    $scope.increaseOffset = function() {
      if ($scope.offset + $scope.limit < $scope.nodes.length) {
        $scope.offset += $scope.limit;
      }

    }
    $scope.decreaseOffset = function() {
      if ($scope.offset > 0) {
        $scope.offset -= $scope.limit;
      }
    }
}]);
