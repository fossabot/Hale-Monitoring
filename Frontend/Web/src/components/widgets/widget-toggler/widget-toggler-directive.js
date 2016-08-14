(function() {
  'use strict';
  angular.module('HaleGUI')
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
