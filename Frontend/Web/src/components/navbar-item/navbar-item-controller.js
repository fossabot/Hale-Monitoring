angular.module('HaleGUI')
  .controller('NavbarItemController', ['$scope', function($scope) {
    $scope.url;
    $scope.icon;
    $scope.label;
    $scope.nav = function() {
      window.location.href = '#/' + $scope.url;
    }
  }]);
