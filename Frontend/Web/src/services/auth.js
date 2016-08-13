angular.module('HaleGUI')
  .factory('Auth', ['$location', '$http', 'store', function($location, $http, store) {
    this.login = function(credentials, onSuccess, onFail) {
      $http({
        method: 'POST',
        data: {
          'username': credentials.username,
          'password': credentials.password
        },
        contentType: 'application/json',
        url: 'http://localhost:8989/api/v1/authentication',
        withCredentials: true
      }).then(function(response) {
        console.dir(response);
        if (response.data.account.id !== null
        && response.data.error === '') {
          console.dir(response.data.account);
          store.set('hale-session', response.data.account);
          onSuccess();
        }
        else {
          onFail();
        }
      })
    }

    this.logout = function() {
      store.remove('hale-session');
    }

    this.validateLogin = function() {
      console.dir(store.get('hale-session'));
      if (store.get('hale-session') === null
        && $location.path() !== "/login") {
          $location.path('/login');
        }
    }

    return this;
  }]);
