import { Component } from '@angular/core';
import { UIRouter } from 'ui-router-ng2';
import { Users } from 'app/api/users';

@Component({
  selector: 'app-user-badge',
  templateUrl: './user-badge.html',
  styleUrls: [ './user-badge.scss']
})
export class UserBadgeComponent {

  user: any;

  constructor(private Users: Users, private uiRouter: UIRouter) {
    this.getUser();
  }

  private getUser(): void {
    this.Users
      .getCurrent()
      .subscribe((user) => this.user = user);
  }

  getGravatarUrl(email: string) {
    return this.Users.getGravatarUrl(email);
  }

  doLogout() {
    this.uiRouter.stateService.go('app.logout');
  }
}
