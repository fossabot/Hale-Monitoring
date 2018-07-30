import { Component } from '@angular/core';

@Component({
  selector: 'app-profile',
  template: `
  <div class="container">
    <div class="row">
      <div class="col">
        <div class="hg-content">
          <ui-view name="profileMenu"></ui-view>
          <ui-view name="profileContent"></ui-view>
        </div>
      </div>
    </div>
  </div>
  `,
})
export class ProfileComponent {}
