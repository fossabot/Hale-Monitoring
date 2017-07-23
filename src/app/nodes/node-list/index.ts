import { Component, Input } from '@angular/core';
import { Nodes } from 'app/api/nodes';

@Component({
  selector: 'app-node-list',
  templateUrl: './node-list.html',
  styleUrls: ['./node-list.scss'],
})
export class NodeListComponent {
  nodes: any[];
  status: {[key: number]: string};

  constructor(private Nodes: Nodes) {
     this.Nodes
      .list()
      .subscribe(
        (d) => this.handleNodeList(d),
        (e) => this.handleError()
      );
  }

  handleError(): void {
    // TODO: Show a toast -SA 2017-07-22
  }

  handleNodeList(nodes: any): void {
    this.nodes = nodes.filter((item: any) => item.configured !== false);
  }

  getStatusString(statusId: number): string {
    return Status[statusId];
  }
}

enum Status {
  na = -1,
  ok = 0,
  warning = 1,
  error = 2,
}
