(function() {
  'use strict';

  angular.module('hale.gui')
    .component('nodesList', {
      templateUrl: './views/lists/nodes.html',
      controller: 'NodesListController'
    })
    .controller('NodesListController', [
        'Nodes',
        'NodeConstants',
        '$location',
        function(Nodes, NodeConstants, $location) {
          var vm = this;

          vm.status = NodeConstants.StatusBg;
          vm.propertyName = 'status';
          vm.reverse = true;
          vm.filter;

          vm.isReverse = function() {
            return vm.reverse == true;
          }

          vm.nav = function(id) {
            $location.path('/nodes/' + id);
          }

          vm.sortBy = function(propertyName) {
            vm.reverse = (vm.propertyName == propertyName) ? !vm.reverse : false;
            vm.propertyName = propertyName;
          }

          function onNodesListed(response) {
            vm.hosts = response;
          }

          Nodes.List(onNodesListed);
    }]);
})();
