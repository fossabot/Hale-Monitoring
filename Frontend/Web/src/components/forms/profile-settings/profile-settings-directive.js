angular.module('HaleGUI')
  .directive('profileSettings', function() {
    return {
      templateUrl: './views/settings/profile.html',
      controller: 'ProfileSettingsController'
    }
  })
