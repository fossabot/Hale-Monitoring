import { IComponentOptions } from 'angular';

const template = require('./navbar.html');
require('./navbar.scss');

export class NavbarComponent implements IComponentOptions {
  templateUrl: any;
  controller: any;

  constructor() {
    this.templateUrl = template;
    this.controller = NavbarController;
  }
}

export class NavbarController {
  minimized: boolean;
}