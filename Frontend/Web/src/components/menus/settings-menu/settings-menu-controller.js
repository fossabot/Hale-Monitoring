angular.module('HaleGUI')
  .controller('SettingsMenuController', ['$scope', '$location', function($scope, $location) {
    $scope.isActive = function(url) {
      return ($location.path() === url);
    }
    $scope.nav = function(url) {
      $location.path(url);
    }
  }]);
