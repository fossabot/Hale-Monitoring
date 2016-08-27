(function() {
  'use strict';

  angular.module('hale.gui')
    .component('dashboard', {
      templateUrl: './views/partials/dashboard.html',
      controller: [
        '$scope',
        'Widgets',
        function($scope, Widgets) {
          var vm = this;

          vm.widgets = Widgets.List();
          vm.moveCallback = function() {
            Widgets.Save();
          };
      }]
    });
})();
