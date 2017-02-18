import * as angular from 'angular';

export default ['storeProvider', 'toastrConfig', AppConfig];

function AppConfig(storeProvider, toastrConfig) {
  storeProvider.setStore('sessionStorage');
      angular.extend(toastrConfig, {
        positionClass: 'toast-top-center',
        target: 'body',
        maxOpened: 0
  })
}
