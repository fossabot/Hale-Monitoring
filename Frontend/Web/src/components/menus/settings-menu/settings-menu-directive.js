angular.module('HaleGUI')
  .directive('settingsMenu', function() {
    return {
      templateUrl: './views/partials/submenu.html',
      controller: 'SettingsMenuController',
      scope: {
        
      }
    };
  })
