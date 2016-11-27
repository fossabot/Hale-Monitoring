var template = require('./monitored-nodes-list.html');

angular.module('hale.nodes')
    .component('monitoredNodesList', {
      templateUrl: template,
      controller: MonitoredNodesListController
    })

MonitoredNodesListController.$inject = [ 'Nodes', 'NodeConstants', 'NgTableParams' ];
function MonitoredNodesListController(Nodes, NodeConstants, NgTableParams) {
  const vm = this;

  vm.status = NodeConstants.StatusBg;

  function _activate() {
    vm.promise = Nodes.list().then((nodes) => {
      vm.nodes = nodes;
      vm.nodes = nodes.filter((item) => { return item.configured === true; });

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