angular.module('HaleGUI')
  .controller('ProfileSettingsController', ['$scope', function($scope) {
    $scope.username = "simskij";
    $scope.email = "simon.aronsson@outlook.com";
    $scope.fullname = "simon aronsson";
    $scope.title = "Profile Settings";
    $scope.description = "We already know that you are a fancy cat! To make sure that your team members know that as well, make sure the information below is correct and up to date.";
  }]);
