import { IHttpService } from 'angular';

export class NodesService {
  constructor(private $http: IHttpService) {}

  list() {
    return this.$http({
      method: 'GET',
      url: 'http://localhost:8989/api/v1/hosts/',
      withCredentials: true
    }).then(response => response.data);
  }

  get(id) {
    return this.$http({
      method: 'GET',
      url: 'http://localhost:8989/api/v1/hosts/' + id,
      withCredentials: true
    }).then(response => response.data);
  }
  update(host) {
    return this.$http({
      method: 'POST',
      url: 'http://localhost:8989/api/v1/hosts/' + host.id,
      withCredentials: true,
      data: host
    }).then(response => response.data);
  }
}
