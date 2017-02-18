import { IComponentOptions } from 'angular';

const template = require('./badge.html');

export class BadgeComponent implements IComponentOptions {
  templateUrl: any;
  controller: any;

  constructor() {
    this.templateUrl = template;
    this.controller = BadgeController;
  }
}

export class BadgeController {

}