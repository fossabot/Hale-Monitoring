const template = require('./navbar.html');
require('./navbar.scss');

angular.module('hale.shared')
    .component('navbar', {
      templateUrl: template,
      controller: NavbarController
    });

function NavbarController() {
  const vm = this;

  vm.toggleSize = toggleSize;
  
  function toggleSize() {
    vm.minimized = !vm.minimized;
  }
}