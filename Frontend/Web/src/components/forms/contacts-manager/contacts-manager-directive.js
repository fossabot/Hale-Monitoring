(function() {
  'use strict';

  angular.module('HaleGUI')
    .directive('contactsManager', function() {
      return {
        templateUrl: './views/forms/contacts-manager.html',
        controller: 'ContactsManagerController',
        scope: {
          'contactType' : '=hgContacts',
        },
        restrict: 'AE'
      }
    })
})();
