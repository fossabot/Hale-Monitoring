(function() {
  'use strict';

  angular.module('HaleGUI')
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
