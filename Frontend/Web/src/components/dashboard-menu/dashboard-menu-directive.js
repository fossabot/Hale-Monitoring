angular.module('HaleGUI')
  .directive('dashboardMenu', function() {
    return {
      templateUrl: './views/partials/dashboard-menu.html',
      controller: 'DashboardMenuController'
    }
  });
