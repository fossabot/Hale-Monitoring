angular.module('HaleGUI')
  .config(function(storeProvider) {
      storeProvider.setStore('sessionStorage');
  })
  .constant('Const', {
    NodeStatus: {
      '-1': 'Not Applicable',
       '0': 'OK',
       '1': 'Warning',
       '2': 'Critical'
     },
     NodeStatusBg: {
       '-1': 'gray',
        '0': 'green',
        '1': 'orange',
        '2': 'red'
      }
  })
