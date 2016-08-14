(function() {
  'use strict';

  angular.module('HaleGUI')
  .controller('LoginFormController', ['$scope', 'Auth', '$location', function($scope, Auth, $location) {
    $scope.credentials = {
      username: '',
      password: ''
    };
    $scope.onLoginSuccess = function() {
      $location.path('/');
    }
    $scope.onLoginFailed = function(reason) {
      console.log('Nepp!');
    }

    $scope.doLogin = function() {
      Auth.login($scope.credentials, $scope.onLoginSuccess, $scope.onLoginFailed);
    }

  }]);
})();
