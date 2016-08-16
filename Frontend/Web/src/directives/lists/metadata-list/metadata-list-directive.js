(function() {
  'use strict';
angular.module('hale.gui')
  .directive('metadataList', function() {
    return {
      templateUrl: './views/lists/metadata.html',
      controller: 'MetadataListController'
    }
  })

})();
