import { NgModule } from '@angular/core';
import { UIView, UIRouterModule, UIRouter, Transition } from 'ui-router-ng2';
import { FormsModule } from '@angular/forms';

import Auth from 'app/api/auth';

import Login from './login';

const states = [
  {
    name: 'app.login',
    url: '/login',
    views: {
      'main@': {
        component: Login
      }
    },
    anon: true
  },
  {
    name: 'app.logout',
    url: '/logout',
    onEnter: (trans: Transition) => {
      trans
        .injector()
        .get(Auth)
        .logout()
        .subscribe(
          _ => trans.router.stateService.transitionTo('app.login')
        );
      ;
    },
  },
]

@NgModule({
  imports: [
    FormsModule,
    UIRouterModule.forChild({
      states: states
    })
  ],
  declarations: [
    Login,
  ]
})
export default class AuthModule {}
