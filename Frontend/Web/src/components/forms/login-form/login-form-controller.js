angular.module('HaleGUI')
  .controller('LoginFormController', ['$scope', 'Auth', function($scope, Auth) {
    $scope.doLogin = function() {
      Auth.login('foo', 'bar');
    }
  }]);
