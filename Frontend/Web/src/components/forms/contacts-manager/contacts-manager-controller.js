angular.module('HaleGUI')
  .controller('ContactsManagerController', ['$scope', 'Users', function($scope, Users) {

    var user = Users.Get(2);

    if ($scope.contactType == 'emails') {
      $scope.contacts = user.emails;
    }
    else if ($scope.contactType == 'phones') {
      $scope.contacts = user.phones;
    }

    $scope.messages = {
      'contactPrimary' : {

        'text' : 'Your primary contact has been changed',
        'type' : 'alert-info',
        'visible': false
      },
      'contactDeleted': {
        'text' : 'The contact has been deleted from your account.',
        'type' : 'alert-danger',
        'visible' : false
      },
      'contactDeleted': {
        'text' : 'The contact has been added to your account.',
        'type' : 'alert-success',
        'visible' : false
      }
    };

    $scope.persist = function() {
      if ($scope.contactType == 'emails') {
        user.emails = $scope.contacts;
      }
      if ($scope.contactType == 'phones') {
        user.phones = $scope.contacts;
      }
      Users.Update(user);
    };

    $scope.hideAllMessages = function() {
      for (i=0;i<$scope.messages.length;i++) {
        $scope.messages[i].visible = false;
      }
    };

    $scope.deleteContact = function(contact) {
      for (i=0;i<$scope.contacts.length;i++) {
        if ($scope.contacts[i].id == contact.id) {
          $scope.contacts.splice(i, 1);
        }
      }
      $scope.persist();
      $scope.hideAllMessages();
      $scope.messages.contactDeleted.visible = true;
    }
    $scope.changePrimary = function(contact) {
      for (i=0;i<$scope.contacts.length;i++) {
        if ($scope.contacts[i].id != contact.id) {
          $scope.contacts[i].primary = false;
        }
        else {
          $scope.contacts[i].primary = true;
        }
      }
      $scope.persist();
      $scope.hideAllMessages();
      $scope.messages.contactPrimary.visible = true;
    }
    $scope.addContact = function(name) {
      $scope.persist();
      $scope.hideAllMessages();
      $scope.messages.contactCreated.visible = true;
    }

  }])
