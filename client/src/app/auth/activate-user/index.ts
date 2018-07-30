import { Component } from '@angular/core';
import { NotificationsService } from 'angular2-notifications';
import { Auth, IActivationAttempt } from 'app/api/auth';
import { StateService } from '@uirouter/angular';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-activate-user',
  templateUrl: './activate-user.html',
  styleUrls: [
    './activate-user.scss'
  ]
})
export class ActivateUserComponent {
  constructor(
    private notifications: NotificationsService,
    private Auth: Auth,
    private $state: StateService) {}

  doActivate(form: NgForm) {
    const activation: IActivationAttempt = {
      activationPassword: form.value.activationCode,
      newPassword: form.value.password,
      username: form.value.userName,
    };

    this.Auth
      .activate(activation)
      .subscribe(
      () => {
        this.notifications.success('Success!', 'Account activated');
        this.$state.go('app.login');
      },
      () => {
        this.notifications.error('Failed!', 'Account activation failed');
      }
    );
  }
}
