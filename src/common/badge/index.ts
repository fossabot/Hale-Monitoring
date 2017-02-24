import { IComponentOptions } from 'angular';

const template = require('./badge.html');
require('./badge.scss');

export class BadgeComponent implements IComponentOptions {
  templateUrl: any;
  controller: any;

  constructor() {
    this.templateUrl = template;
    this.controller = [
      'Users',
      BadgeController
    ];
  }
}

export class BadgeController {
  user: any;

  constructor(private Users: any) {}

  $onInit() {
    this.getUser();
  }
  private getUser(): void {
    this.Users
      .getCurrent()
      .then((user: any) => {
        this.user = user;
      });
  }
}