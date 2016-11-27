const template = require('./unconfigured-nodes-list.html');
require('./unconfigured-nodes-list.scss');

angular.module('hale.nodes')
    .component('unconfiguredNodesList', {
        templateUrl: template,
        controller: UnconfiguredNodesListController
    });

UnconfiguredNodesListController.$inject = [ 'Nodes', 'NgTableParams', 'toastr' ];
function UnconfiguredNodesListController(Nodes, NgTableParams, toastr) {
  const vm = this;

  vm.callbacks = {
    'save': _doSave,
    'cancel': _doCancel,
    'ban': _doBan
  };

  function _doCancel(host) {
    vm.nodes
      .filter((node) => { return node.id === host.id})[0]
      .edit = false;
  }

  function _doSave(host) {
    Nodes.update(host)
      .then(() => {
        toastr.success('Host has been configured.');
        _loadData();
      })
      .catch(() => {
        toastr.error('Could not save host configuration.');
      });
  }

  function _doBan(host) {
    host.blocked = true;
    Nodes.update(host)
      .then(() => {
        toastr.success('Host has been blocked.');
        _loadData();
      })
      .catch(() => {
        toastr.error('Host could not be blocked.')
      });
    
  }

  function _loadData() {
    vm.promise = Nodes.list().then((nodes) => {
      vm.nodes = nodes.filter((item) => { return item.configured === false && item.blocked === false; });
      vm.tableParams = new NgTableParams({
        count: 20
      }, {
        counts: [],
        dataset: vm.nodes
      });
    });
  }

  function _activate() {
    _loadData();
  }

  _activate();
 
}