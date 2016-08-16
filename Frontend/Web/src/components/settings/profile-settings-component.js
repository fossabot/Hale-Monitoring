angular.module('hale.gui')
  .component('profileSettings', {
    templateUrl: './views/settings/profile.html',
    controller: 'ProfileSettingsController'
  })
  .controller('ProfileSettingsController', function() {
      var vm = this;
      vm.username = "simskij";
      vm.email = "simon.aronsson@outlook.com";
      vm.fullname = "simon aronsson";
      vm.title = "Profile Settings";
      vm.description = "We already know that you are a fancy cat! To make sure that your team members know that as well, make sure the information below is correct and up to date.";
  });
