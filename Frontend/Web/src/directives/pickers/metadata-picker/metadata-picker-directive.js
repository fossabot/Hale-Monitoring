(function() {
  'use strict';

  angular.module('hale.gui')
    .directive('metadataTemplatePicker', function() {
      return {
        templateUrl: './views/pickers/metadata-template-picker.html',
        controller: 'MetadataTemplatePickerController'
      }
    });
})();
