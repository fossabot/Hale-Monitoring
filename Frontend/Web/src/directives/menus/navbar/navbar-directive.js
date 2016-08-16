(function() {
  'use strict';

  angular.module('hale.gui')
    .directive('navbar', function() {
      return {
        templateUrl: './views/partials/navbar.html',
        controller: 'NavbarController',
        restriction: 'AE'
      }
    })
})();
