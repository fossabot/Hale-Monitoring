
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms/forms';
import { Users } from 'app/api/users';
import { NotificationsService } from 'angular2-notifications';
import { StateService } from '@uirouter/angular';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.html',
  styleUrls: [
    './create-user.scss'
  ]
})

export class CreateUserComponent {
  // isCollapsed: boolean = true;

  constructor(
    private Users: Users,
    private notifications: NotificationsService,
    private state: StateService
  ) {}

  doCreate(form: NgForm): void {
    const newUser = {
      userName: form.value.userName,
      email: form.value.email,
      fullName: form.value.fullName,
      password: form.value.password
    };
    this.Users
      .create(newUser)
      .subscribe(
        () => {
          this.notifications.success(`Success!`, 'User account created');
          this.state.go('app.hale.admin.users', {}, {reload: true});
        },
        () => {
          this.notifications.error('Warning!', 'User creation failed');
        });
    console.log(form);
  }
}
