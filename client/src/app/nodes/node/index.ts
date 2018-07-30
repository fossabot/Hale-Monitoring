import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-node',
  templateUrl: './node.html',
  styleUrls: ['./node.scss'],
})
export class NodeComponent {
  @Input() node: any;

  constructor() {}

  getStatus() {
    return Status[this.node.status];
  }

}

enum Status {
  na = -1,
  ok = 0,
  warning = 1,
  error = 2,
}
