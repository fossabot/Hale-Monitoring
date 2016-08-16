(function() {
  'use strict';

  angular.module('hale.gui')
  .controller('WidgetTogglerController', ['$scope', 'Widgets', function($scope, Widgets) {
    $scope.widgets = Widgets.List();
  }])
})();
