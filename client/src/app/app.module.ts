import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { UIView, UIRouterModule, UIRouter } from '@uirouter/angular';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TransitionService } from '@uirouter/angular';
import { SimpleNotificationsModule } from 'angular2-notifications';

import { ProfileModule } from './profile';
import { AuthModule } from './auth';
import { ApiModule } from './api';
import { CommonModule } from './common';
import { ConfigsModule } from './configs';
import { NodesModule } from './nodes';
import { AdminModule } from './admin';

import { Auth } from './api/auth';
import { Users } from './api/users';

import { NavbarComponent } from './common/navbar';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    ProfileModule,
    ApiModule,
    AdminModule,
    AuthModule,
    CommonModule,
    ConfigsModule,
    NodesModule,
    BrowserModule,
    BrowserAnimationsModule,
    SimpleNotificationsModule.forRoot(),
    FormsModule,
    HttpModule,
    NgbModule.forRoot(),
    UIRouterModule.forRoot({
      states: [
        { name: 'app', abstract: true },
        { name: 'app.hale',
          abstract: true,
          views: {
            'nav@': {
              component: NavbarComponent
             }
          },
          resolve: [
            {
              token: 'currentUser',
              deps: [Users],
              resolveFn: resolveCurrentUser
            }
          ]
        }
      ],
      otherwise: { state: 'app.login' },
      useHash: true,
      config: configureAppRouter,
    })
  ],
  bootstrap: [AppComponent]
})

export class AppModule {
  constructor(private transitionService: TransitionService) {
    transitionService.onStart({}, (tran) => {
      document.getElementById('loader').classList.add('visible');
      tran.promise.then(() => {
        document.getElementById('loader').classList.remove('visible');
      });
    });
  }
}

export function resolveCurrentUser(users: Users) {
  return users
    .getCurrent()
    .toPromise();
}

export function configureAppRouter(router: UIRouter) {
    const criteria = { entering: (state: any) => state.anon === undefined && state.admin === undefined };
    const adminCriteria = { entering: (state: any) => state.anon === undefined && state.admin};


    router.transitionService.onBefore(adminCriteria, (transition) => {
      const state = transition.router.stateService;
      const auth = transition.injector().get(Auth);
      return auth
        .checkAdmin()
        .subscribe(
          undefined,
          () => {
            console.log('Received 401 unauthorized on admin route. Redirecting to nodes list');
            return state.target('app.hale.nodes');
          }
        );

    });

    router.transitionService.onBefore(criteria, (transition) => {
      const state = transition.router.stateService;
      const auth = transition.injector().get(Auth);
      return auth
        .check()
        .subscribe(
          undefined,
          () => {
            console.log('Received 401 unauthorized on protected route. Redirecting to login page.');
            return state.target('app.login');
          }
        );
    });
}
