(function() {
  'use strict';
  angular.module('HaleGUI')
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
