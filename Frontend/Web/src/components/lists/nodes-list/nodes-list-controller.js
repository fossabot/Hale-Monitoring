angular.module('HaleGUI')
  .controller('NodesListController', ['$scope', 'Nodes', 'Const', function($scope, Nodes, Const) {

    $scope.status = Const.NodeStatusBg;

    function onNodesListed(response) {
      $scope.hosts = response;
    }

    Nodes.List(onNodesListed);

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

  }]);
