(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('MainController', ['$scope', 'Auth', 'store', function($scope, Auth, store) {
      Auth.validateLogin();
    }]);
})();
