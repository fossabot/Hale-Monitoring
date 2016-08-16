(function() {
  'use strict';

  angular.module('hale.gui')
  .directive('badge', function() {
    return {
      templateUrl: './views/partials/badge.html',
      resticted: 'E',
      controller: 'BadgeController'
        }
  })
})();
