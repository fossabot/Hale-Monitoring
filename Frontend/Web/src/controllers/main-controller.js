angular.module('HaleGUI')
  .controller('MainController', ['$scope', 'Auth', 'store', function($scope, Auth, store) {
    Auth.validateLogin();
  }]);
