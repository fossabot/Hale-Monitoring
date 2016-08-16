(function() {
  'use strict';

  angular.module('hale.gui')
  .controller('SubmenuController', ['$scope', '$location', function($scope, $location) {
    $scope.isActive = function(url) {
      return ($location.path() === url);
    }
    $scope.nav = function(url) {
      console.log('Navigating to ' + url);
      $location.path(url);
    }
  }])

})();
