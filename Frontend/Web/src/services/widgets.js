(function() {
  'use strict';
  angular.module('HaleGUI')
    .factory('Widgets', function() {
      var widgets = [
          {
            'title' : 'Host Summary',
            'view'  : './views/widgets/host-summary.html',
            'size'  : '12',
            'enabled' : true
          },
          {
            'title' : 'Status Chart',
            'view'  : './views/widgets/status-chart.html',
            'size'  : '3',
            'enabled' : true
          },
          {
            'title' : 'Twitter',
            'view'  : '',
            'size'  : '3',
            'enabled' : true
          },
          {
            'title' : 'Node Map',
            'view'  : '',
            'size'  : '3',
            'enabled' : true
          },
          {
            'title' : 'Quote',
            'view'  : '',
            'size'  : '3',
            'enabled' : true
          },
          {
            'title' : 'Outtages',
            'view'  : '',
            'size'  : '12',
            'enabled' : true
          }
        ];

      this.List = function() {
        return widgets;
      }

      this.Save = function() {
        console.log('HaleGUI.Widgets - Mockup Save Triggered.');
        console.log('Current state of model:');
        console.dir(widgets);
      }

      return this;

    });
  })();
