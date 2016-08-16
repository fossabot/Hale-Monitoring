(function() {
  'use strict';

  angular.module('hale.gui')
  .component('loginForm', {
    templateUrl: 'views/partials/login-form.html',
    controller: [
      'Auth',
      '$location',
      function(Auth, $location) {
        var vm = this;

        vm.messages = {
          'loginFailed': {
            text: 'The username and/or password you entered are incorrect.',
            class: 'alert-danger',
            visible: false
          }
        }

        vm.credentials = {
          username: '',
          password: ''
        }

        vm.doLogin = function() {
          Auth.login(vm.credentials, onLoginSuccess, onLoginFailed);
        }

        function onLoginSuccess() {
          $location.path('/');
        }

        function onLoginFailed(reason) {
          vm.messages.loginFailed.visible = true;
        }
      }]
  });
})();
