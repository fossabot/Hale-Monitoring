import { IComponentOptions } from 'angular';

const template = require('./template.html');

export default class hgNodeTimestamps {
  bindings: {[key: string]: string};
  templateUrl: any;
  controller: any;

  constructor() {
    this.bindings = {
      node: '=hgNode'
    }
    this.templateUrl = template;
    this.controller = hgNodeTimestampsController;
  }
}

class hgNodeTimestampsController {
  node: any;
}