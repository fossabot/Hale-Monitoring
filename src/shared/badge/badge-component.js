(function() {
  'use strict';

  angular.module('hale.gui')
    .component('badge', {
      templateUrl: './views/partials/badge.html',
      controller: ['$location', 'Auth', function($location, Auth) {
      var vm = this;

      vm.profile = function() { $location.path('/settings/profile'); };
      vm.logout  = function() {
        Auth.logout();
        $location.path('/login');
      }
    }]
  });
})();
