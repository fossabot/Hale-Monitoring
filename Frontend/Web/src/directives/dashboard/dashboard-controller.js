(function() {
  'use strict';

  angular.module('hale.gui')
  .controller('DashboardController', ['$scope', 'Widgets', function($scope, Widgets) {
    $scope.widgets = Widgets.List();

   $scope.moveCallback = function() {
     Widgets.Save();
   };

  }]);
})();
