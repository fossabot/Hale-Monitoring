var template = require('./nodes-list.html');

angular.module('hale.nodes')
    .component('nodesList', {
      templateUrl: template,
      controller: NodesListController
    })

NodesListController.$inject = [ 'Nodes', 'NodeConstants' ];
function NodesListController(Nodes, NodeConstants) {
  const vm = this;

  vm.sortBy = sortBy;

  vm.status = NodeConstants.StatusBg;
  vm.propertyName = 'status';
  vm.reverse = true;
  vm.filter;

  function sortBy(propertyName) {
    vm.reverse = (vm.propertyName == propertyName) ? !vm.reverse : false;
    vm.propertyName = propertyName;
  }
  function _activate() {
    vm.promise = Nodes.list().then((nodes) => {
      vm.nodes = nodes;
    });
  }

  _activate();

}