(function() {
  'use strict';

  angular.module('hale.gui')
  .directive('dashboard', function() {
    return {
      templateUrl: './views/partials/dashboard.html',
      restriction: 'AE',
      controller: 'DashboardController'
    }
  })
})();
