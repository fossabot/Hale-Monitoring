angular.module('HaleGUI')
  .controller('NavbarItemController', ['$scope', '$location', function($scope, $location) {
    $scope.url;
    $scope.icon;
    $scope.label;
    $scope.nav = function() {
      $location.path('/' + $scope.url);
    }
    $scope.isActive = function() {
      return ($location.path() === $scope.url);
    }

  }]);
