import { Component, Input, OnInit } from '@angular/core';
import { Configs } from 'app/api/configs';


@Component({
  selector: 'app-config',
  templateUrl: './config.html',
  styleUrls: [
    './config.scss'
  ]
})
export class ConfigComponent implements OnInit {

  @Input() config: any;
  @Input() id: number;

  original: string;

  constructor(private Configs: Configs) {}

  ngOnInit() {
    this.original = this.config;
  }

  doSave() {
    this.Configs
      .save(this.id, this.config)
      .toPromise()
      .then(() => {
        console.log('Saved!');
      });
  }

}
