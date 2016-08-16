(function() {
  'use strict';

  angular.module('hale.gui')
    .component('contactsManager', {
      bindings: {
        'contactType' : '=hgContacts',
      },
      templateUrl: './views/settings/contact-manager.html',
      controller: 'ContactsManagerController'
    })
    .controller('ContactsManagerController', ['Users',
      function(Users) {
        var vm = this;

        vm.messages = {
          'fetchFailed': {
            'text': 'Could not load contacts. Try reloading the page.',
            'type': 'alert-danger',
            'visible': false
          },
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

        vm.deleteContact = function(contact) {
          for (var i=0;i<vm.contacts.length;i++) {
            if (vm.contacts[i].id == contact.id) {
              vm.contacts.splice(i, 1);
            }
          }
          save();
          vm.messages.contactDeleted.visible = true;
        }
        vm.changePrimary = function(contact) {
          for (var i=0;i<vm.contacts.length;i++) {
            vm.contacts[i].primary = (vm.contacts[i].id === contact.id ? true : false);
          }
          save();
          vm.messages.contactPrimary.visible = true;
        }
        vm.addContact = function(name) {
          save();
          vm.messages.contactCreated.visible = true;
        }

        function save() {
          user.emails = (vm.contactType == 'emails' ? vm.contacts : user.emails);
          user.phones = (vm.contactType == 'phones' ? vm.contacts : user.phones);
          Users.Update(user);

          for (var i=0;i<vm.messages.length;i++) {
            vm.messages[i].visible = false;
          }
        };

        function onGetSuccess(user) {
            vm.contacts = (vm.contactType == 'emails' ? user.emails : (vm.contactType == 'phones' ? user.phones : ''));
        }
        function onGetFailed() {
            vm.messages.fetchFailed.visible = true;
        }

        Users.Get(2, onGetSuccess, onGetFailed);
      }
    ])
})();
