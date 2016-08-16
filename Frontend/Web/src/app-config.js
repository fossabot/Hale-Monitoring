(function() {
  'use strict';
  angular.module('hale.gui')
    .config(function(storeProvider) {
        storeProvider.setStore('sessionStorage');
    })
    .constant('NodeConstants', {
      StatusText: {
        '-1': 'Not Applicable',
         '0': 'OK',
         '1': 'Warning',
         '2': 'Critical'
       },
      StatusBg: {
         '-1': 'gray',
          '0': 'green',
          '1': 'orange',
          '2': 'red'
        }
    });
})();
