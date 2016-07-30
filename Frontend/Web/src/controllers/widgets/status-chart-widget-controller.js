angular.module('HaleGUI')
  .controller('StatusChartWidgetController', ['$scope', 'Nodes', function($scope, Nodes) {
    $scope.upCount   = 0;
    $scope.downCount = 0;
    var nodes = Nodes.List();
    for (i=0;i<nodes.length;i++) {
      if (nodes[i].status == 'up') {
        $scope.upCount++;
      }
      else {
        $scope.downCount++;
      }
    }
    $scope.data = [$scope.upCount, $scope.downCount];
    $scope.labels = ['up', 'down']
  }])
