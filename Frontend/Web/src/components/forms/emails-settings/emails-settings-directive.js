angular.module('HaleGUI')
  .directive('emailsSettings', function() {
    return {
      templateUrl: './views/settings/emails.html',
      controller: 'EmailsSettingsController',
      scope: {

      },
      restrict: 'AE'
    }
  })
