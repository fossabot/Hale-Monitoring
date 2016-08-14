angular.module('HaleGUI')
  .controller('NodesListController', ['$scope', 'Nodes', 'NodeConstants', function($scope, Nodes, NodeConstants) {

    $scope.status = NodeConstants.StatusBg;

    function onNodesListed(response) {
      $scope.hosts = response;
    }

    Nodes.List(onNodesListed);

    $scope.propertyName = 'status';
    $scope.reverse = true;
    $scope.filter;

    $scope.isReverse = function() {
      return $scope.reverse == true;
    }

    $scope.sortBy = function(propertyName) {
      $scope.reverse = ($scope.propertyName == propertyName) ? !$scope.reverse : false;
      $scope.propertyName = propertyName;
    }

  }]);
