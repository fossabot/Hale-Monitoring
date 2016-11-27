angular.module('hale.api')
  .factory('Nodes', NodesService);

NodesService.$inject = ['$http' ];
function NodesService($http) {
  const svc = {};
  
  svc.list = list;
  svc.get = get;
  svc.update = update;

  function list() {
    return $http({
        method: 'GET',
        contentType: 'application/json',
        url: 'http://localhost:8989/api/v1/hosts/',
        withCredentials: true
      })
      .then(response => response.data);
  }

  function get(id) {
    return $http({
      method: 'GET',
      contentType: 'application/json',
      url: 'http://localhost:8989/api/v1/hosts/' + id,
      withCredentials: true
    }).then(response => response.data);
  }

  function update(host) {
    return $http({
      method: 'POST',
      contentType: 'application/json',
      url: 'http://localhost:8989/api/v1/hosts/' + host.id,
      withCredentials: true,
      data: host
    }).then(response => response.data);
  }

  return svc;

}