(function() {
  'use strict';

  angular.module('hale.gui')
    .component('phoneSettings', {
        templateUrl: './views/settings/phone.html',
        controller: 'PhoneSettingsController',
    })
    .controller('PhoneSettingsController', function() {
      var vm = this;
      vm.title = 'Phone Numbers';
      vm.description =
        'Manage the phone numbers associated with your account.';
    })
})();
