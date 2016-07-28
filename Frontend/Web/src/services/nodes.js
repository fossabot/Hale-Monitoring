angular.module('HaleGUI')
  .factory('Nodes', function() {
    // TODO: Switch mock data to loading from API -SA 2016-07-27
    var nodes =

    [
      {
        'id' : 1,
        'name': 'hale-qa-srv01',
        'uri' : 'hale-qa-srv01.io.nav',
        'status': 'up',
        'health': '80',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'id' : 2,
        'name': 'hale-qa-srv02',
        'uri' : 'hale-qa-srv02.io.nav',
        'status': 'down',
        'health': '80',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'id' : 3,
        'name': 'hale-core-srv01',
        'uri' : 'hale-core-srv01.azure.onmicrosoft.com',
        'status': 'up',
        'health': '100',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'id' : 4,
        'name': 'hale-agent-srv01',
        'uri' : 'hale-agent-srv01.azure.onmicrosoft.com',
        'status': 'up',
        'health': '100',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'id' : 5,
        'name': 'hale-agent-srv02',
        'uri' : 'hale-agent-srv02.azure.onmicrosoft.com',
        'status': 'up',
        'health': '100',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'id' : 6,
        'name': 'webserver: simonaronsson.se',
        'uri' : 'simonaronsson.se',
        'status': 'up',
        'health': '100',
        'changed': '2016-01-01 01:01:00'
      }


    ];

    this.List = function() {
      return nodes;
    }

    this.Get = function(id) {
      for (i=0;i<nodes.length;i++) {
        if (nodes[i].id == id) {
          return nodes[i];
        }
      }
    }

    return this;
  });
