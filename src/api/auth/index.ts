export class AuthService {

  $injector: any[] = ['$location', '$http', '$state', 'toastr', this];
  private baseUrl: string = 'http://localhost:8989/api/v1';

  constructor(
    private $location: any,
    private $http: any,
    private $state: any,
    private toastr: any) { }

  login(credentials) {
    return this.$http({
      method: 'POST',
      url: `${this.baseUrl}/authentication`,
      withCredentials: true,
      data: credentials,
    })
    .then(_ => this.$state.go('app.hale.nodes.monitored'))
    .catch(_ => this.toastr.error('The username and/or password provided is incorrect.'));
  }

  logout() {
    return this.$http({
      method: 'DELETE',
      url: `${this.baseUrl}/authentication`,
      withCredentials: true
    })
    .finally(_ => this.$state.go('app.login'));
    
  }

  authenticate() {
    return this.$http({
      method: 'GET',
      contentType: 'application/json',
      url: `${this.baseUrl}/authentication`,
      withCredentials: true
    }).catch(() => {
      this.$state.go('app.login');
    });
  }
}