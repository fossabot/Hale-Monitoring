(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('NodesMenuController', ['$scope', function($scope) {
      $scope.isActive = function(url) {
        return ($location.path() === url);
      }
      $scope.nav = function(url) {
        $location.path(url);
      }


    }])
})();
