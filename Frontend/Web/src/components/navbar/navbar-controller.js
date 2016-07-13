angular.module('HaleGUI')
  .controller('NavbarController', ['$scope', function($scope) {
      $scope.nav = {
        toggle: function() {
          $scope.nav.expanded = ($scope.nav.expanded === true ? false : true);
        },
        expanded: false
      };
  }]);
