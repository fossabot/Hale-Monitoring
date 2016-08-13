angular.module('HaleGUI')
  .controller('HostSummaryWidgetController', ['$scope', 'Nodes', 'Const', function($scope, Nodes, Const) {
    $scope.status = Const.NodeStatusBg;

    function onNodesListed(response) {
      $scope.hosts = response;
    }
    Nodes.List(onNodesListed);

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
