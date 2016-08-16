(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('StatusChartWidgetController', ['$scope', 'Nodes', function($scope, Nodes) {
      $scope.okCount   = 0;
      $scope.warnCount = 0;
      $scope.critCount = 0;
      function onNodesLoaded(response) {
        $scope.hosts = response;
        for (var i=0;i<$scope.hosts.length;i++) {
          if ($scope.hosts[i].status == '0') {
            $scope.okCount++;
          }
          else if ($scope.hosts[i].status == '1') {
            $scope.warnCount++;
          }
          else {
            $scope.critCount++;
          }
        }
        $scope.data = [$scope.okCount, $scope.warnCount, $scope.critCount];
        $scope.labels = ['ok', 'warning', 'critical'];
      }
      Nodes.List(onNodesLoaded);
    }]);
})();
