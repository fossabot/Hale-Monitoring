angular.module('hale.api')
  .factory('Auth', AuthService);

AuthService.$inject = [ '$location', '$http', 'store', '$state', 'toastr' ];
function AuthService($location, $http, store, $state, toastr) {
  
  const svc = {};
  const baseUrl = 'http://localhost:8989/api/v1/';

  svc.login = login;
  svc.logout = logout;
  svc.authenticate = authenticate;

  function login(credentials) {
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
          store.set('hale-session', response.data.account);
          $state.go('app.hale.nodes.monitored');
      },
      () => {
        toastr.error('The username and/or password provided is incorrect.')
      });
    }

  function logout() {
    store.remove('hale-session');
    $state.go('app.login');
  }

  function authenticate() {
    return $http({
      method: 'GET',
      contentType: 'application/json',
      url: baseUrl + 'authentication',
      withCredentials: true
    })
    .catch(() => {
      store.remove('hale-session');
      $state.go('app.login');
    });
  }
  return svc;
}
