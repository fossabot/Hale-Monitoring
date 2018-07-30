import { Component, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Users } from 'app/api/users';
import { NotificationsService } from 'angular2-notifications';
import { StateService } from '@uirouter/angular';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.html',
  styleUrls: [
    './edit-user.scss'
  ]
})
export class EditUserComponent {
  @Input() user: any;

  constructor(
    private Users: Users,
    private notifications: NotificationsService,
    private $state: StateService,
  ) {}

  doUpdate(form: NgForm) {
    console.log(form);
    this.Users
      .updateUser(
        {
          id: this.user.id,
          fullName: form.value.fullName,
          email: form.value.email,
          activated: form.value.activated,
          enabled: form.value.enabled,
        })
      .subscribe(
        () => {
          this.notifications.success('Success!', 'User updates saved');
          this.$state.go('app.hale.admin.users', {}, { reload: true });
        },
        () => {
          this.notifications.error('Error!', 'User update failed');
        }
      );
  }
}
