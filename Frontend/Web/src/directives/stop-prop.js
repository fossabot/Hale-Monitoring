(function() {
  'use strict';

  angular.module('hale.gui')
    .directive('stopProp', function() {
      return function(scope, element, attrs) {
          $(element).click(function(event) {
              event.stopPropagation();
          });
      }
  })
})();
