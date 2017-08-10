import { Component } from '@angular/core';
import { Users} from 'app/api/users';

@Component({
  selector: 'app-edit-profile',
  styleUrls: ['./edit-profile.scss'],
  templateUrl: './edit-profile.html',
})
export class EditProfileComponent {
  user: IUser;
  constructor(private users: Users) {
    this.users
      .getCurrent()
      .subscribe(
        (user: IUser) => {
          this.user = user;
        }
      );
  }
}

interface IUser {
  id: number;
  username: string;
  email: string;
  fullName: string;
  activated: boolean;
  enabled: boolean;
  created: Date;
  createdBy: number;
  modified: Date;
  modifiedBy: number;
  // TODO: add account details to interface -SA 2017-07-24
}
