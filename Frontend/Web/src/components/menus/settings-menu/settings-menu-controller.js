angular.module('HaleGUI')
  .controller('SettingsMenuController', ['$scope', '$location', function($scope, $location) {
    $scope.isActive = function(url) {
      return ($location.path() === url);
    }
    $scope.nav = function(url) {
      $location.path(url);
    }

    $scope.items = [
      {
        'label' : 'Profile',
        'url' : '/settings/profile'
      },
      {
        'label' : 'Emails',
        'url' : '/settings/emails'
      },
      {
        'label' : 'Phones',
        'url' : '/settings/phones'
      },
      {
        'label' : 'Dashboard',
        'url' : '/settings/dashboard'
      }
    ]
  }]);
