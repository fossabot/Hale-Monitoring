angular.module('HaleGUI')
  .controller('NavbarController', ['$scope', '$location', function($scope, $location) {
    $scope.showNavbar = function() {
      return $location.path() !== '/login';
    }
    $scope.isActive = function(url) {
      return ($location.path() === '/' + url);
    }
  }]);
