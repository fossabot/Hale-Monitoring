(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('NodesController', ['nodes', function(nodes) {
      var vm = this;
      vm.nodes = nodes;
    }]);
})();
