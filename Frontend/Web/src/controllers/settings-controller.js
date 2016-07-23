angular.module('HaleGUI')
  .controller('SettingsController', ['$scope', '$routeParams', function($scope, $routeParams) {
    $scope.title = 'Settings';
    $scope.description = '';
    $scope.renderSection = function(name) {
      return ($routeParams.section === name);
    }
  }])
