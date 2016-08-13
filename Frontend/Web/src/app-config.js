angular.module('HaleGUI')
  .config(function(storeProvider) {
      storeProvider.setStore('sessionStorage');
  })
