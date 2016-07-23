angular.module('HaleGUI')
  .directive('loginform', function() {
    return {
      templateUrl: './views/partials/login-form.html',
      controller: 'LoginFormController',
      restriction: 'AE'
    }
  });
