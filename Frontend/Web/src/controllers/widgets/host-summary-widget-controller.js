angular.module('HaleGUI')
  .controller('HostSummaryWidgetController', ['$scope', 'Nodes' , function($scope, Nodes) {
    $scope.nodes = Nodes.List();

    $scope.statusUpCount = 0;
    for (var i=0;i<$scope.nodes.length;i++) {
      if ($scope.nodes[i].status == 'up') {
        $scope.statusUpCount++;
      }
    }

    $scope.statusUpCountPct = ($scope.statusUpCount/$scope.nodes.length*100).toFixed(1);
  }
])
