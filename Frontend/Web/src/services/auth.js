angular.module('HaleGUI')
  .factory('Auth', ['$location', function($location) {
    this.getLoginStatus = function() {
      return this.isLoggedIn;
    }
    this.isLoggedIn = false;

    this.login = function(username, password) {
      this.isLoggedIn = true;
      $location.path('/dashboard');
    }

    this.logout = function() {
      this.isLoggedIn = false;
    }

    this.validateLogin = function() {
      if (this.isLoggedIn === false
        && $location.path() !== "/login") {
          $location.path('/login');
        }
    }

    return this;
  }]);
