angular.module('hale.gui')
    .controller('NavbarController', function() {
      var vm = this;

      vm.toggleSize = () => {
        vm.minimized = !vm.minimized;
      }
    });