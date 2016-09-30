(function() {
  'use strict';

  angular.module('hale.gui')
    .factory('Nodes', ['$http', function($http) {
      var self = this;

      this.list = function() {
        return $http({
          method: 'GET',
          contentType: 'application/json',
          url: 'http://localhost:8989/api/v1/hosts/',
          withCredentials: true
        }).then(function(response) {
          return response.data;
        });
      }

      this.get = function(id) {
        return $http({
          method: 'GET',
          contentType: 'application/json',
          url: 'http://localhost:8989/api/v1/hosts/' + id,
          withCredentials: true
        }).then(function(response) {
          return response.data;
        });
      }

      return this;
    }]);
})();
