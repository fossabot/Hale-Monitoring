(function() {
  'use strict';

  angular.module('HaleGUI')
    .factory('Users', function() {
      // TODO: Switch mock data to loading from API -SA 2016-07-27
      var users =
      [
        {
          'id' : 1,
          'username' : 'simskij',
          'fullname' : 'simon aronsson',
          'phones':
          [
            { 'id'      : 1, 'value'   : '+46 (0) 72-51 61 361', 'primary' : true },
            { 'id'      : 2, 'value'   : '+46 (0) 71-11 41 362', 'primary' : false },
          ],
          'emails':
          [
            { 'id'      : 1, 'value' : 'simon.aronsson@outlook.com', 'primary' : true  },
            { 'id'      : 2, 'value' : 'simon.aronsson@gmail.com'  , 'primary' : false },
            { 'id'      : 3, 'value' : 'carl.gustav@kungahuset.se' , 'primary' : false },
          ]
        },
        {
          'id' : 2,
          'username' : 'parti',
          'fullname' : 'nils måsén',
          'phones' :
          [
            { 'id'      : 1, 'value'   : '+46 (0) 72-51 61 361', 'primary' : true },
            { 'id'      : 2, 'value'   : '+46 (0) 71-11 41 362', 'primary' : false },
          ],
          'emails':
          [
            { 'id'      : 1, 'value' : 'nils.masen@outlook.com', 'primary' : true  },
            { 'id'      : 2, 'value' : 'nils.masen@gmail.com'  , 'primary' : false },
            { 'id'      : 3, 'value' : 'victoria@kungahuset.se' , 'primary' : false },
          ]
        }
      ];

      this.Update = function(user) {
        console.log('HaleGUI.Users: Mock Update Triggered.');
        for (i=0;i<users.length;i++)
        {
            if (users[i].id == user.id)
            {
              console.dir(users[i])
              users[i] = user;
            }
        }
        console.dir(user);
      }

      this.List = function() {
        return users;
      };

      this.Get = function(id) {
        for (i=0;i<users.length;i++)
        {
            if (users[i].id == id)
            {
              return users[i];
            }
        }
      };

      return this;
    });
})();
