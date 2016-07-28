angular.module('HaleGUI')
  .directive('adminMenu', function() {
    return {
      templateUrl: './views/partials/submenu.html',
      controller: 'AdminMenuController'
    };
  })
