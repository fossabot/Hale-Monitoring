angular.module('HaleGUI')
  .directive('dashboardSettings', function() {
    return {
      templateUrl: './views/settings/dashboard.html',
      controller: 'DashboardSettingsController'
    }
  })
