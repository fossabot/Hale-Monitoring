import { Component, OnInit, Input } from '@angular/core';
import { Results } from 'app/api/results';

@Component({
  selector: 'app-node-checks',
  templateUrl: './node-checks.html',
  styleUrls: ['./node-checks.scss']
})
export class NodeChecksComponent implements OnInit {
  @Input() nodeId: number;
  results: any[];

  constructor(private Results: Results) {
  }

  ngOnInit() {
    this.Results
      .list(this.nodeId)
      .subscribe((result) => {
        this.results = result;
      });
  }
}
