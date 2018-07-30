import { Component } from '@angular/core';

@Component({
  selector: 'app-admin',
  template: `
  <div class="container">
    <div class="row">
      <div class="col">
        <div class="hg-content">
          <ui-view name="adminMenu"></ui-view>
          <ui-view name="adminContent"></ui-view>
        </div>
      </div>
    </div>
  </div>
  `
})
export class AdminComponent {}
