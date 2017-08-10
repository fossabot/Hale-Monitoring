import { Component, Input } from '@angular/core';
import { UIRouter } from '@uirouter/angular';
import { Users } from 'app/api/users';

@Component({
  selector: 'app-user-badge',
  templateUrl: './user-badge.html',
  styleUrls: [ './user-badge.scss']
})
export class UserBadgeComponent {

  @Input() user: any;

  constructor(private Users: Users, private uiRouter: UIRouter) {
  }

  getGravatarUrl(email: string) {
    return this.Users.getGravatarUrl(email || '');
  }

  doLogout() {
    this.uiRouter.stateService.go('app.logout');
  }
}
