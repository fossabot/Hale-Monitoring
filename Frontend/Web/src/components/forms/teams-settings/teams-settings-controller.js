(function() {
  'use strict';

  angular.module('HaleGUI')
  .controller('TeamsSettingsController', ['$scope', function($scope) {
    $scope.title = 'Team Settings';
    $scope.description =
      'Teams are used as a target for assigning privileges and routing alerts ' +
      'to a certain selection of persons. For example, you may create a ' +
      'stakeholders team with limited possibilities to interact with the system, ' +
      'other than in read only. You may also create a group for DBAs, to whom ' +
      'you then assign all the database related alerts under the Integrations tab.'
  }]);
})();
