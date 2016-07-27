angular.module('HaleGUI')
  .controller('NavbarController', ['$scope', '$location', '$document', function($scope, $location, $document) {
    $scope.hasSubmenu = function() {
      return window.document.getElementsByClassName("hg-submenu")[0] !== undefined;
    }
    $scope.showNavbar = function() {
      return $location.path() !== '/login';
    }
    $scope.isActive = function(url) {
      return ($location.path() === '/' + url ? true : $location.path().split('/')[1] == url);
    }
  }]);
