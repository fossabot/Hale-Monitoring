import { Component, Input } from '@angular/core';

@Component({
  selector: 'node-timestamps',
  templateUrl: './node-timestamps.html',
  styleUrls: [ './node-timestamps.scss']
})
export default class NodeTimestamps {
  @Input() node: any;
}
