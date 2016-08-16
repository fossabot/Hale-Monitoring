(function() {
  'use strict';
  angular.module('hale.gui')
    .directive('settingsMenu', function() {
      return {
        templateUrl: './views/partials/submenu.html',
        controller: 'SettingsMenuController',
        scope: {

        }
      };
    })

})();
