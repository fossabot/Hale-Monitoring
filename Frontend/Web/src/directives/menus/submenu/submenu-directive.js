(function() {
  'use strict';

  angular.module('hale.gui')
    .directive('subMenu', function() {
      return {
        templateUrl: './views/partials/submenu.html',
        controller: 'SubmenuController',
        scope: {
          'items' : '=hgSubmenuItems'
        },
        restrict: 'AE'
      }
    })
})();
