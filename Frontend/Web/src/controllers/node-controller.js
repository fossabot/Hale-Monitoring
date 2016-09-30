(function() {
  'use strict';
  angular.module('hale.gui')
    .controller('NodeController', ['node', function(node) {
      var vm = this;

      vm.node = node;
    }]);
})();
