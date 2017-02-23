import { IComponentOptions, IPromise } from 'angular';

const template = require('./monitored-nodes-list.html');

export class MonitoredNodesListComponent implements IComponentOptions {
  templateUrl: any;
  controller: any;

  constructor() {
    this.templateUrl = template;
    this.controller = [
      'Nodes',
      'NodeConstants',
      'NgTableParams',
      MonitoredNodesListController
    ];
  }
}

export class MonitoredNodesListController {
  status: any;
  promise: IPromise<any>;
  nodes: any;
  tableParams: any;

  constructor(
    private Nodes: any,
    private NodeConstants: any,
    private NgTableParams: any) {}
  
  $onInit() {
    this.status = this.NodeConstants.StatusBg;
    this.promise = this.Nodes
      .list()
      .then((nodes: any) => {
        this.nodes = nodes.filter((item: any) => item.configured);
        this.tableParams = new this.NgTableParams({ count: 20}, { counts: [], dataset: this.nodes});
      });
  }
}
