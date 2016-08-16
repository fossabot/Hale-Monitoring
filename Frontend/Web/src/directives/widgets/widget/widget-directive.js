(function() {
  'use strict';
  angular.module('hale.gui')
    .directive('widget', function() {
      return {
        templateUrl: './views/partials/widget.html',
        controller: 'WidgetController',
        restriction: 'AE',
        scope: {
          'size' : '@hgWidgetSize',
          'title' : '@hgWidgetTitle',
          'view' : '@hgWidgetView'
        }
      }
    })
})();
