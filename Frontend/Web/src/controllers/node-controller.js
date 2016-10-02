(function() {
  'use strict';
  angular.module('hale.gui')
    .controller('NodeController', ['node', 'NodeConstants', function(node, NodeConstants) {
      var vm = this;
      vm.node = node;
      vm.status = NodeConstants.StatusBg;
      
      vm.friendlifyDate = function(date) {
        return new Date(date).toDateString();
      }
      console.dir(vm.node);
    }]);
})();
