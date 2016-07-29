angular.module('HaleGUI')
  .controller('MainController', ['$scope', 'Auth', '$location', function($scope, Auth, $location) {
    Auth.validateLogin();
    $scope.isLoggedIn = Auth.getLoginStatus();
    console.log($scope.isLoggedIn);


  }]);
