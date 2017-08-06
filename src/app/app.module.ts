import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { UIView, UIRouterModule, UIRouter } from '@uirouter/angular';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';

import { AdminModule } from './admin';
import { AuthModule } from './auth';
import { ApiModule } from './api';
import { CommonModule } from './common';
import { ConfigsModule } from './configs';
import { NodesModule } from './nodes';

import { Auth } from './api/auth';
import { NavbarComponent } from './common/navbar';
import { TransitionService } from '@uirouter/angular';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    AdminModule,
    ApiModule,
    AuthModule,
    CommonModule,
    ConfigsModule,
    NodesModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    NgbModule.forRoot(),
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
