(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('NavbarController', function() {
      var vm = this;

      vm.toggleSize = function() {
        vm.minimized = !vm.minimized;
      }
    });
})();
