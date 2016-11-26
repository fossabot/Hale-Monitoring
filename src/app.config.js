angular.module('hale.gui')
  .config(AppConfig)
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
  
AppConfig.$inject = [ 'storeProvider', 'toastrConfig' ];
function AppConfig(storeProvider, toastrConfig) {
      storeProvider.setStore('sessionStorage');
      angular.extend(toastrConfig, {
        positionClass: 'toast-top-center',
        target: 'body',
        maxOpened: 0
      })

}
  