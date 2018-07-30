import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-node-basics',
  templateUrl: './node-basics.html',
  styleUrls: [ './node-basics.scss']
})
export class NodeBasicsComponent {
  @Input() node: any;
}
