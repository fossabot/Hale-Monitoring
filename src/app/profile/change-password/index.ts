import { Component } from '@angular/core';
import { NgForm, FormControl } from '@angular/forms';
import { Auth } from 'app/api/auth';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.html',
  styleUrls: [ './change-password.scss'],
})

export class ChangePasswordComponent {
  showInfo: boolean = true;
  constructor(
    private Auth: Auth,
    private notifications: NotificationsService) {}

  doChangePassword(form: NgForm) {
    this.Auth
      .changePassword(
        form.value.oldPassword,
        form.value.password,
      )
      .toPromise()
      .then(() => {
        this.notifications.success('Success!', 'Password changed.');
      })
      .catch(() => {
        this.notifications.error('Fail', 'Password change failed.');
      });

  }

}
