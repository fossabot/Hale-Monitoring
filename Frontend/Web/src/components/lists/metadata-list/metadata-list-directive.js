angular.module('HaleGUI')
  .directive('metadataList', function() {
    return {
      templateUrl: './views/lists/metadata.html',
      controller: 'MetadataListController'
    }
  })
