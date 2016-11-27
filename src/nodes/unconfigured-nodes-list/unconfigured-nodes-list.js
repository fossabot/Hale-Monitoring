const template = require('./unconfigured-nodes-list.html');
require('./unconfigured-nodes-list.scss');

angular.module('hale.nodes')
    .component('unconfiguredNodesList', {
        templateUrl: template,
        controller: UnconfiguredNodesListController
    });

UnconfiguredNodesListController.$inject = [ 'Nodes', 'NgTableParams' ];
function UnconfiguredNodesListController(Nodes, NgTableParams) {
  const vm = this;

  function _activate() {
    vm.promise = Nodes.list().then((nodes) => {
      vm.nodes = nodes.filter((item) => { return item.configured === false; });
      vm.tableParams = new NgTableParams({
        count: 20
      }, {
        counts: [],
        dataset: vm.nodes
      });
    });
  }

  _activate();
 
}