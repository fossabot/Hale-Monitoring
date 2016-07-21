angular.module('HaleGUI')
  .controller('DashboardMenuController', ['$scope', '$location', function($scope, $location) {
    $scope.nav = {
      'dashboardSettings' : function() {
        $location.path('/settings/dashboard');
      }
    }
  }]);
