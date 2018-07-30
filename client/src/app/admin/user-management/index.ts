import { Component, Input, OnInit } from '@angular/core';
import { Users } from 'app/api/users';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.html',
  styleUrls: [
    './user-management.scss'
  ]
})

export class UserManagementComponent implements OnInit {
  @Input() users: any;

  constructor(private Users: Users) {}

  ngOnInit() {
    console.log(this.users);
  }

  getGravatarUrl(email: string): string {
    return this.Users.getGravatarUrl(email);
  }

  getStatusText(user: any): string {
    if (user.enabled && user.activated) {
      return 'Active';
    }
    if (!user.enabled) {
      return 'Disabled';
    }
    return 'Awaiting activation';
  }

  getStatusClass(user: any): string {
    if (user.enabled && user.activated) {
      return 'ok';
    }
    if (!user.enabled) {
      return 'error';
    }
    return 'ongoing';
  }
}
