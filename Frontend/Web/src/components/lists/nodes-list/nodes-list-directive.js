angular.module('HaleGUI')
  .directive('nodesList', function() {
    return {
      templateUrl: './views/lists/nodes.html',
      controller: 'NodesListController'
    }
  })
