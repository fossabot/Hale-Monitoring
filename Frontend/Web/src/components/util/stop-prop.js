(function() {
  'use strict';

  angular.module('HaleGUI')
    .directive('stopProp', function() {
      return function(scope, element, attrs) {
          $(element).click(function(event) {
              event.stopPropagation();
          });
      }
  })
})();
