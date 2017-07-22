import { Component, Input } from '@angular/core';

@Component({
  selector: 'node',
  templateUrl: './node.html',
  styleUrls: ['./node.scss'],
})
export default class Node {
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
