import { IComponentOptions, IPromise } from 'angular';

const template = require('./unconfigured-nodes-list.html');
require('./unconfigured-nodes-list.scss');

export class UnconfiguredNodesListComponent implements IComponentOptions {
  templateUrl: any;
  controller: any;

  constructor() {
    this.templateUrl = template;
    this.controller = [
      'Nodes',
      'NgTableParams',
      'toastr',
      UnconfiguredNodesListController
    ];
  }
}



export class UnconfiguredNodesListController {

  nodes: any;
  promise: IPromise<any>;
  tableParams: any;
  callbacks: {[key: string]: any};

  constructor(
    private Nodes: any,
    private NgTableParams: any,
    private toastr: any) {}

  $onInit() {
    this.loadData();
    this.callbacks = {
      save: this.doSave,
      cancel: this.doCancel,
      ban: this.doBan
    };
  }

  private doCancel(host: any) {
    this.nodes.filter((node: any) => node.id === host.id)[0].edit = false;
  }

  private doSave(host: any) {

    this.Nodes
      .update(host)
      .then(() => {
        this.toastr.success('Host has been configured.');
        this.loadData();
      })
      .catch(() => {
        this.toastr.error('Could not save host configuration.');
      });
  }

  private doBan(host: any) {
    host.blocked = true;
    this.Nodes
      .update(host)
      .then(() => {
        this.toastr.success('Host has been blocked.');
        this.loadData();
      })
      .catch(() => {
        this.toastr.error('Host could not be blocked.')
      });

  }

  private loadData() {
    this.promise = this.Nodes
      .list()
      .then((nodes: any) => {
        this.nodes = nodes.filter((item: any) => item.configured === false && item.blocked === false);
        this.tableParams = new this.NgTableParams({ count: 20 }, { counts: [], dataset: this.nodes });
    });
  }
}
