(function() {
  'use strict';

  angular.module('HaleGUI')
    .controller('SettingsController', ['$scope', '$routeParams', function($scope, $routeParams) {
      $scope.title = 'Settings';
      $scope.description = '';
      $scope.renderSection = function(name) {
        return ($routeParams.section === name);
      };

      $scope.submenuItems = [
        {
          'label' : 'Profile',
          'url' : '/settings/profile'
        },
        {
          'label' : 'Dashboard',
          'url' : '/settings/dashboard'
        },
        {
          'label': 'Contacts',
          'items': [
            {
              'label' : 'Emails',
              'url' : '/settings/emails'
            },
            {
              'label' : 'Phones',
              'url' : '/settings/phones'
            }
          ]
        }
      ]
    }]);
  })();
