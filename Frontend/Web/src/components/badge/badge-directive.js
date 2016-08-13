(function() {
  'use strict';

  angular.module('HaleGUI')
  .directive('badge', function() {
    return {
      templateUrl: './views/partials/badge.html',
      resticted: 'E',
      controller: 'BadgeController'
        }
  })
})();
