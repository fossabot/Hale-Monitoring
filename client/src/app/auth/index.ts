import { NgModule } from '@angular/core';
import { UIView, UIRouterModule, UIRouter, Transition } from '@uirouter/angular';
import { FormsModule } from '@angular/forms';
import { StatesModule } from '@uirouter/angular';
import { Auth } from 'app/api/auth';
import { LoginComponent } from './login';
import { ActivateUserComponent } from './activate-user';
import { CommonModule } from 'app/common';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    UIRouterModule.forChild(<StatesModule>{
      states: [
        {
          name: 'app.login',
          url: '/login',
          views: {
            'main@': {
              component: LoginComponent
            }
          },
          anon: true
        },
        {
          name: 'app.logout',
          url: '/logout',
          onEnter: doLogout,
        },
        {
          name: 'app.activation',
          url: '/activate',
          views: {
            'main@': {
              component: ActivateUserComponent
            }
          }
        }
      ]
    })
  ],
  declarations: [

    LoginComponent,
    ActivateUserComponent,
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
