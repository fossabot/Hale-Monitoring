import { Ng2StateDeclaration } from '@uirouter/angular';
import { Transition } from '@uirouter/core/lib';

import { Users } from 'app/api/users';

import { AdminComponent } from './admin.component';
import { AdminMenuComponent } from './admin-menu';
import { UserManagementComponent } from './user-management';
import { CreateUserComponent } from 'app/admin/create-user';
import { EditUserComponent } from 'app/admin/edit-user';

interface AdminStateDeclaration extends Ng2StateDeclaration {
  admin?: boolean;
}

export const AdminRoutes: AdminStateDeclaration[] = [
  {
    url: '/admin',
    abstract: true,
    admin: true,
    name: 'app.hale.admin',
    views: {
      'main@': {
        component: AdminComponent
      },
      'adminMenu@app.hale.admin': {
        component: AdminMenuComponent
      }
    },
  },
  {
    url: '/users',
    name: 'app.hale.admin.users',
    views: {
      'adminContent@app.hale.admin': {
        component: UserManagementComponent
      }
    },
    resolve: [
      {
        token: 'users',
        deps: [Users],
        resolveFn: resolveUsers
      }
    ]
  },
  {
    url: '/users/create',
    name: 'app.hale.admin.users.create',
    views: {
      'adminContent@app.hale.admin': {
        component: CreateUserComponent
      }
    }
  },
  {
    url: '/users/:id/edit',
    name: 'app.hale.admin.users.edit',
    views: {
      'adminContent@app.hale.admin': {
        component: EditUserComponent
      }
    },
    resolve: [
      {
        token: 'user',
        resolveFn: resolveUserFromId,
        deps: [Transition, Users],
      }
    ]
  }
];

export function resolveUserFromId(Transition: Transition, Users: Users) {
  return Users
    .getById(Transition.params().id)
    .toPromise();
}

export function resolveUsers(Users: Users) {
  return Users
    .list()
    .toPromise();
}
