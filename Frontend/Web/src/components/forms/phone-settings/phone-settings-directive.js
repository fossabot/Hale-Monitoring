angular.module('HaleGUI')
  .directive('phonesSettings', function() {
    return {
      templateUrl: './views/settings/phones.html',
      controller: 'PhonesSettingsController',
      scope: {

      },
      restrict: 'AE'
    }
  })
