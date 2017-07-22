import { Component, Input } from '@angular/core';

@Component({
  selector: 'node-basics',
  templateUrl: './node-basics.html',
  styleUrls: [ './node-basics.scss']
})
export default class NodeBasics {
  @Input() node: any;
}
