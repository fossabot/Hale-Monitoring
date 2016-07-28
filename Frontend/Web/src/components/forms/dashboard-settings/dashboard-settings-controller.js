angular.module('HaleGUI')
  .controller('DashboardSettingsController', ['$scope', function($scope) {
    $scope.title =
     "Dashboard Settings";
    $scope.description =
      "Select the widgets that you would like to present on the dashboard screen." +
      "If none is selected, the dashboard will be empty and unusable (duh)." +
      "Drag and drop the widgets to order, enable or disable them.";

    $scope.widgets = [
      {
        'title' : 'Node Summary',
        'enabled' : true,
        'icon'  : 'fa-list',
        'description':
          'Displays a summary of all hosts registered in Hale Monitoring and ' +
          'their current status. This is a condensed version of what you see ' +
          'in the hosts tab.'
      }
    ];
  }]);
