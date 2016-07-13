angular.module('HaleGUI')
  .directive('dashboard', function() {
    return {
      templateUrl: './views/partials/dashboard.html',
      restriction: 'AE',
      controller: 'DashboardController'
    }
  })
