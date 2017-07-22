import { Component } from '@angular/core';
import { UIRouter } from 'ui-router-ng2';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.scss']
})

export class NavbarComponent {
  minimized: boolean;
  nav: {[key: string]: string}[];

  constructor(private uiRouter: UIRouter) {
    this.nav = [
      { state: 'app.hale.nodes', title: 'Nodes'},
      { state: 'app.hale.nodes', title: 'Admin'},
    ];
  }

  goToState(stateName: string) {
    this.uiRouter.stateService.go(stateName);
  }
}
