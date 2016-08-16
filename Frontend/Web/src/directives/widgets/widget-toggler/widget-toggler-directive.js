(function() {
  'use strict';
  angular.module('hale.gui')
    .directive('widgetToggler', function() {
      return {
        templateUrl: './views/partials/widget-toggler.html',
        controller: 'WidgetTogglerController',
        scope: {

        },
        restrict: 'AE'
      }
    })
})();
