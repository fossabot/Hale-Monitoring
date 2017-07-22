import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { UIView, UIRouterModule, UIRouter } from '@uirouter/angular';

import { AppComponent } from './app.component';

import { AuthModule } from './auth';
import { ApiModule } from './api';
import { CommonModule } from './common';
import { NodesModule } from './nodes';

import { Auth } from './api/auth';
import { NavbarComponent } from './common/navbar';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    ApiModule,
    AuthModule,
    CommonModule,
    NodesModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    UIRouterModule.forRoot({
      states: [
        { name: 'app', abstract: true },
        { name: 'app.hale', abstract: true, views: { 'nav@': { component: NavbarComponent }}}
      ],
      otherwise: { state: 'app.login' },
      useHash: true,
      config: configureAppRouter,
    })
  ],
  bootstrap: [AppComponent]
})

export class AppModule {}

export function configureAppRouter(router: UIRouter) {
    let criteria = { entering: (state) => state.anon === undefined }
    router.transitionService.onBefore(criteria, (transition) => {
      let $state = transition.router.stateService;
      let auth = transition.injector().get(Auth);
      return auth
        .check()
        .subscribe(
          undefined,
          () => {
            console.log('Received 401 unauthorized on protected route. Redirecting to login page.');
            $state.transitionTo('app.login')}
        );
    });
}
