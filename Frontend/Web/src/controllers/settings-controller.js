(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('SettingsController', ['$scope', function($scope) {
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
          'label' : 'Emails',
          'url' : '/settings/email'
        },
        {
          'label' : 'Phones',
          'url' : '/settings/phone'
        }
      ]
    }]);
  })();
