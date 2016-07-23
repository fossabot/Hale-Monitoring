angular.module('HaleGUI')
  .directive('addMetadataModal', function() {
    return {
      templateUrl: './views/modals/add-metadata-attribute.html',
      controller: 'AddMetadataModalController'
    }
  })
