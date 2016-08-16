(function() {
  'use strict';

  angular.module('hale.gui')
    .factory('Nodes', ['$http', function($http) {
      var self = this;
      this.hosts = [];
      this.Load = function() {
        return $http({
          method: 'GET',
          contentType: 'application/json',
          url: 'http://localhost:8989/api/v1/hosts',
          withCredentials: true
        }).then(function(response) {
          self.hosts = response.data;
        });
      }
      this.List = function(callback) {
        if (this.hosts.length < 1) {
          this.Load().then(function() {
            callback(self.hosts);
          });
        }
        callback(this.hosts);
      }

      this.Get = function(id) {
        for (i=0;i<this.hosts.length;i++) {
          if (this.hosts[i].id == id) {
            return this.hosts[i];
          }
        }
      }

      return this;
    }]);
})();
