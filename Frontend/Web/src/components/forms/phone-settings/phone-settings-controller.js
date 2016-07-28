angular.module('HaleGUI')
  .controller('PhonesSettingsController', ['$scope', function($scope) {
    $scope.title = 'Phone Numbers';
    $scope.description =
      'Manage the phone numbers associated with your account.';
  }]);
