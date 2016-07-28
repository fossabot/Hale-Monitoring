angular.module('HaleGUI')
  .controller('AdminMenuController', ['$scope', '$location', function($scope, $location) {
    $scope.isActive = function(url) {
      return ($location.path() === url);
    }
    $scope.nav = function(url) {
      $location.path(url);
    }

    $scope.items = [
      {
        'label' : 'Teams',
        'url' : '/admin/teams'
      },
      {
        'label' : 'Metadata',
        'url' : '/admin/metadata'
      },
      {
        'label' : 'Agent',
        'url' : '/admin/agent'
      },

      {
        'label' : 'Core',
        'url' : '/admin/core'
      },
      {
        'label' : 'Checkpacks',
        'url' : '/admin/checkpacks'
      },
      {
        'label' : 'Integrations',
        'url' : '/admin/integrations'
      },
      {
        'label' : 'Security',
        'url' : '/admin/security'
      },
    ]
  }]);
