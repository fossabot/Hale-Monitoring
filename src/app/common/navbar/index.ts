import { Component, OnInit, Input } from '@angular/core';
import { UIRouter } from '@uirouter/angular';
import { Users } from 'app/api/users';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.scss']
})

export class NavbarComponent implements OnInit {
  minimized: boolean;
  nav: {[key: string]: string}[];

  @Input() currentUser: any;
  user: any;
  constructor(
    private uiRouter: UIRouter,
    private Users: Users) {}

  ngOnInit() {
    this.user = this.currentUser;
    this.nav = [
      {
        state: 'app.hale.nodes',
        title: 'Nodes',
        display: true
      },
      {
        state: 'app.hale.configs',
        title: 'Configs',
        display: true
      },
      {
        state: 'app.hale.admin.users',
        title: 'Admin',
        display: this.user.isAdmin
      },
    ];
  }

  goToState(stateName: string) {
    this.uiRouter.stateService.go(stateName);
  }
}
