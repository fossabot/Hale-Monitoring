(function() {
  'use strict';
  angular.module('hale.gui')
    .component('emailSettings', {
      templateUrl: './views/settings/email.html',
      controller: 'EmailSettingsController',
    })
    .controller('EmailSettingsController', function() {
      var vm = this;
      vm.title =
        "Email Settings";
      vm.description =
        "Administration of the email adresses associated with your account. " +
        "You may have as many email adresses as you want connected to your account " +
        "but only one of them may be flagged as primary at any given time.";

    });

})();
