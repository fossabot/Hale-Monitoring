(function() {
  'use strict';

angular.module('hale.gui')
  .directive('adminMenu', function() {
    return {
      templateUrl: './views/partials/submenu.html',
      controller: 'AdminMenuController'
    };
  })
})();
