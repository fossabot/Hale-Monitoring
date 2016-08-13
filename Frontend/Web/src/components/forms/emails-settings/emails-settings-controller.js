(function() {
  'use strict';

  angular.module('HaleGUI')
  .controller('EmailsSettingsController', ['$scope', function($scope) {
    $scope.title = "Email Settings";
    $scope.description =
      "Administration of the email adresses associated with your account. " +
      "You may have as many email adresses as you want connected to your account " +
      "but only one of them may be flagged as primary at any given time.";


  }])
})();
