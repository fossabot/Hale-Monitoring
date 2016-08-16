(function() {
  'use strict';

angular.module('hale.gui')
  .directive('addMetadataModal', function() {
    return {
      templateUrl: './views/modals/add-metadata-attribute.html',
      controller: 'AddMetadataModalController'
    }
  })
})();
