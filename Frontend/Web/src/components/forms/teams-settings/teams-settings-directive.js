angular.module('HaleGUI')
  .directive('teamsSettings', function() {
    return {
      templateUrl: './views/settings/teams.html',
      controller: 'TeamsSettingsController'
    }
  })
