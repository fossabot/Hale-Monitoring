import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UIRouterModule } from '@uirouter/angular';
import { FormsModule } from '@angular/forms';

import { UserBadgeComponent } from './user-badge';
import { NavbarComponent } from './navbar';
import { DevButtonComponent } from './dev-button';
import { ValidatePasswordDirective } from './validate-password';
import { ValidateEqualsDirective } from './validate-equals';
import { ValidateUsernameDirective } from './validate-username';

@NgModule({
  imports: [
    UIRouterModule.forChild(),
    BrowserModule,
    NgbModule,
    FormsModule
  ],
  declarations: [
    UserBadgeComponent,
    NavbarComponent,
    DevButtonComponent,
    ValidatePasswordDirective,
    ValidateEqualsDirective,
    ValidateUsernameDirective,
  ],
  exports: [
    DevButtonComponent,
    ValidatePasswordDirective,
    ValidateEqualsDirective,
    ValidateUsernameDirective,
  ]
})
export class CommonModule {
}
