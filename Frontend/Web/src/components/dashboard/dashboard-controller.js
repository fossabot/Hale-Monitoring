angular.module('HaleGUI')
  .controller('DashboardController', ['$scope', 'Widgets', function($scope, Widgets) {
    $scope.widgets = Widgets.List();

   $scope.moveCallback = function() {
     Widgets.Save();
   };

  }]);
