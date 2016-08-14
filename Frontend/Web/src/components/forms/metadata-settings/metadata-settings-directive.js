(function() {
  'use strict';

  angular.module('HaleGUI')
  .directive('metadataSettings', function() {
    return {
      templateUrl: './views/settings/metadata.html',
      controller: 'MetadataSettingsController'
    }
  })
})();
