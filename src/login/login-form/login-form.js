const template = require('./login-form.html');
require('./login-form.scss');

angular.module('hale.login')
  .component('loginForm', {
    templateUrl: template,
    controller: LoginFormController
  });
  
LoginFormController.$inject = [ 'Auth', '$location']
function LoginFormController(Auth, $location) {
  var vm = this;
  
  vm.doLogin = doLogin;

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

  function doLogin() {
    Auth.login(vm.credentials);
  }
}