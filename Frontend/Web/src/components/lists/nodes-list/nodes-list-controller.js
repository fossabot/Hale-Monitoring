angular.module('HaleGUI')
  .controller('NodesListController', ['$scope', function($scope) {
    $scope.propertyName = 'name';
    $scope.reverse = false;
    $scope.filter;
    $scope.isReverse = function() {
      return $scope.reverse == true;
    }

    $scope.sortBy = function(propertyName) {
      $scope.reverse = ($scope.propertyName == propertyName) ? !$scope.reverse : false;
      $scope.propertyName = propertyName;
    }

    $scope.hosts = [
      {
        'name': 'hale-qa-srv01',
        'uri' : 'hale-qa-srv01.io.nav',
        'status': 'up',
        'health': '80',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'name': 'hale-qa-srv02',
        'uri' : 'hale-qa-srv02.io.nav',
        'status': 'down',
        'health': '80',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'name': 'hale-core-srv01',
        'uri' : 'hale-core-srv01.azure.onmicrosoft.com',
        'status': 'up',
        'health': '100',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'name': 'hale-agent-srv01',
        'uri' : 'hale-agent-srv01.azure.onmicrosoft.com',
        'status': 'up',
        'health': '100',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'name': 'hale-agent-srv02',
        'uri' : 'hale-agent-srv02.azure.onmicrosoft.com',
        'status': 'up',
        'health': '100',
        'changed': '2016-01-01 01:01:00'
      },
      {
        'name': 'webserver: simonaronsson.se',
        'uri' : 'simonaronsson.se',
        'status': 'up',
        'health': '100',
        'changed': '2016-01-01 01:01:00'
      }


    ];

  }])
