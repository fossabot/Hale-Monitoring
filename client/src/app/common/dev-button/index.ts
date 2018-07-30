import { Component, Input, OnInit } from '@angular/core';

import { environment as env } from 'environments/environment';

 @Component({
   selector: 'app-dev-button',
   templateUrl: './dev-button.html',
   styleUrls: [
     './dev-button.scss'
   ]
 })

export class DevButtonComponent implements OnInit {
  @Input() text: string;
  @Input() json: any;
  jsonText: string;

  isCollapsed: boolean = true;
  production: boolean;

  constructor() {}

  ngOnInit(): void {
    this.production = env.production;
    this.jsonText = JSON.stringify(this.json, null, 2);
  }
}
