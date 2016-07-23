angular.module('HaleGUI')
  .directive('metadataTemplatePicker', function() {
    return {
      templateUrl: './views/pickers/metadata-template-picker.html',
      controller: 'MetadataTemplatePickerController'
    }
  });
