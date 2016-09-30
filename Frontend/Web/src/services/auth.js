(function() {
  'use strict';

  angular.module('hale.gui')
    .factory('Auth', ['$location', '$http', 'store', '$state', function($location, $http, store, $state) {
      var baseUrl = 'http://localhost:8989/api/v1/';

      this.login = function(credentials) {
        return $http({
          method: 'POST',
          data: {
            'username': credentials.username,
            'password': credentials.password
          },
          contentType: 'application/json',
          url: baseUrl + 'authentication',
          withCredentials: true
        })
        .then(
          (response) => {
            if (response.data.account !== null && response.data.error === '') {
              store.set('hale-session', response.data.account);
              $state.go('app.hale.dashboard');
            }
            else {
              $state.go('app.login.failed');
            }
          },
          () => {
            $state.go('app.login.unavailable');
        });
      }

      this.logout = function() {
        store.remove('hale-session');
        $state.go('app.login');
      }

      this.authorize = function() {
        return $http({
          method: 'GET',
          contentType: 'application/json',
          url: baseUrl + 'authentication',
          withCredentials: true
        })
        .then((response) => {

        }, () => {
          store.remove('hale-session');
          $state.go('app.login');
        });
      }
      return this;
    }]);
})();
