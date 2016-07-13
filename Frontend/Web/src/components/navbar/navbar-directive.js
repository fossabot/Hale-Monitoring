angular.module('HaleGUI')
  .directive('navbar', function() {
    return {
      templateUrl: './views/partials/navbar.html',
      controller: 'NavbarController',
      restriction: 'AE'
    }
  })
