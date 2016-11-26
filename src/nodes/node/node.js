const template = require('./node.html');
require('./node.scss');

angular.module('hale.nodes')
  .component('node', {
    templateUrl: template,
    controller: NodeController
  });

NodeController.$inject = [ 'Nodes', 'NodeConstants', '$stateParams' ];
function NodeController(Nodes, NodeConstants, $stateParams) {
  const vm = this;

  vm.status = NodeConstants.StatusBg;
      
  vm.getFriendlyDate = getFriendlyDate;

  function getFriendlyDate(date) {
    return new Date(date).toDateString();
  }

  function _activate() {
    vm.promise = Nodes.get($stateParams.id).then((node) => {
      vm.node = node;
    });
  }

  _activate();
}
