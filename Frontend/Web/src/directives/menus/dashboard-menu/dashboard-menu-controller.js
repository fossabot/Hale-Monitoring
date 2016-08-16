(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('DashboardMenuController', ['$scope', '$location', function($scope, $location) {
      $scope.nav = {
        'dashboardSettings' : function() {
          $location.path('/settings/dashboard');
        }
      }
    }]);
})();
