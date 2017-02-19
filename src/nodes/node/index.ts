import { IPromise, IComponentOptions } from 'angular';

const template = require('./node.html');
require('./node.scss');

export class NodeComponent {
  templateUrl: string;
  controller: any;

  constructor() {
    this.templateUrl = template;
    this.controller = [
      'Nodes',
      'NodeConstants',
      '$stateParams',
      NodeController
    ];
  }
}

export class NodeController {

  status: any;
  promise: IPromise<any>;
  node: any;

  constructor(
    private Nodes: any,
    private NodeConstants: any,
    private $stateParams: any) {}

  $onInit() {
    this.promise = this.Nodes
      .get(this.$stateParams.id)
      .then((node: any) => {
        this.node = node;
      });
  }

  getStatus() {
    return this.NodeConstants.Status[this.node ? this.node.status : '-1'];
  }
}