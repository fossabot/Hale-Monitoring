(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('BadgeController', ['$scope', '$location', 'Auth', function($scope, $location, Auth) {
      $scope.profile = function() {
          $location.path('/settings/profile');
      };
      $scope.logout = function() {
        Auth.logout();
        $location.path('/login');
      }
    }]);
})();
