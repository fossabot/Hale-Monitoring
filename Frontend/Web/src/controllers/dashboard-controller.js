(function() {
  'use strict';

  angular.module('hale.gui')
    .controller('DashboardController', [ 'Widgets', function(Widgets) {
          var vm = this;

          vm.widgets = Widgets.List();
          vm.moveCallback = function() {
            Widgets.Save();
          };
      }
    ]);
})();
