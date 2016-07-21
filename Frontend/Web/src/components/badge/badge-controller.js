angular.module('HaleGUI')
.controller('BadgeController', ['$scope', '$location', function($scope, $location) {
  $scope.profile = function() {
      $location.path('/settings/profile');
  };
  $scope.logout = function() {

        $location.path('/logout');
  };

}]);
