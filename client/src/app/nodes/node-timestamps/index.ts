import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-node-timestamps',
  templateUrl: './node-timestamps.html',
  styleUrls: [ './node-timestamps.scss']
})
export class NodeTimestampsComponent {
  @Input() node: any;
}
