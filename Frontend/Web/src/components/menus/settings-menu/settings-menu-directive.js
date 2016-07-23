angular.module('HaleGUI')
  .directive('settingsMenu', function() {
    return {
      templateUrl: './views/partials/settings-menu.html',
      controller: 'SettingsMenuController'
    };
  })
