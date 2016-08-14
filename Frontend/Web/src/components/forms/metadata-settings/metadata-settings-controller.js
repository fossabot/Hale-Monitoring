(function() {
  'use strict';

  angular.module('HaleGUI')
  .controller('MetadataSettingsController', ['$scope', function($scope) {
    $scope.title = 'Metadata Templates';
    $scope.description =
      'Metadata is additional attributes you may define to extend ' +
      'the standard data forms so that they correspond to your organizations ' +
      'reality. Each metadata attribute is tied to a certain template that ' +
      'will also be the context in which it wil be rendered and/or edited.';
  }]);
})();
