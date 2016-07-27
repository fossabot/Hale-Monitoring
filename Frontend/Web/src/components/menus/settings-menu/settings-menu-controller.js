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
        'label' : 'Teams',
        'url' : '/settings/teams'
      },
      {
        'label' : 'Dashboard',
        'url' : '/settings/dashboard'
      },
      {
        'label' : 'Metadata',
        'url' : '/settings/metadata'
      },
      {
        'label' : 'Agent',
        'url' : '/settings/agent'
      },

      {
        'label' : 'Core',
        'url' : '/settings/core'
      },
      {
        'label' : 'Checkpacks',
        'url' : '/settings/checkpacks'
      },
      {
        'label' : 'Integrations',
        'url' : '/settings/integrations'
      },
      {
        'label' : 'Security',
        'url' : '/settings/security'
      },
    ]
  }]);
