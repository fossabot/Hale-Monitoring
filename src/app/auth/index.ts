import { NgModule } from '@angular/core';
import { UIView, UIRouterModule, UIRouter, Transition } from '@uirouter/angular';
import { FormsModule } from '@angular/forms';
import { StatesModule } from '@uirouter/angular';
import { Auth } from 'app/api/auth';
import { Login } from './login';

@NgModule({
  imports: [
    FormsModule,
    UIRouterModule.forChild(<StatesModule>{
      states: [
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
          onEnter: doLogout,
        },
      ]
    })
  ],
  declarations: [
    Login,
  ]
})
export class AuthModule { }

export function doLogout(trans: Transition) {
  trans
    .injector()
    .get(Auth)
    .logout()
    .subscribe(() => trans.router.stateService.transitionTo('app.login'));
  ;
}
