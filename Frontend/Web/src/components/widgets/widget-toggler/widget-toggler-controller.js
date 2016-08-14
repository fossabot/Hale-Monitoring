(function() {
  'use strict';

  angular.module('HaleGUI')
  .controller('WidgetTogglerController', ['$scope', 'Widgets', function($scope, Widgets) {
    $scope.widgets = Widgets.List();
  }])
})();
