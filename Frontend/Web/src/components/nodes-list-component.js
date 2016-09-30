(function() {
  'use strict';

  angular.module('hale.gui')
    .component('nodesList', {
      templateUrl: './views/lists/nodes.html',
      controller: 'NodesListController',
      bindings: {
        'nodes': '='
      }
    })
    .controller('NodesListController', [
        'NodeConstants',
        function(NodeConstants) {
          var vm = this;

          vm.status = NodeConstants.StatusBg;
          vm.propertyName = 'status';
          vm.reverse = true;
          vm.filter;

          vm.isReverse = function() {
            return vm.reverse == true;
          }

          vm.sortBy = function(propertyName) {
            vm.reverse = (vm.propertyName == propertyName) ? !vm.reverse : false;
            vm.propertyName = propertyName;
          }
    }]);
})();
