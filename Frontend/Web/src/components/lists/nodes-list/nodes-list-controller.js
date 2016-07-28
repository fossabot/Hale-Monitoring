angular.module('HaleGUI')
  .controller('NodesListController', ['$scope', 'Nodes', function($scope, Nodes) {
    $scope.propertyName = 'name';
    $scope.reverse = false;
    $scope.filter;
    $scope.isReverse = function() {
      return $scope.reverse == true;
    }

    $scope.sortBy = function(propertyName) {
      $scope.reverse = ($scope.propertyName == propertyName) ? !$scope.reverse : false;
      $scope.propertyName = propertyName;
    }

    $scope.hosts = Nodes.List();
  }]);
