import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { UserBadgeComponent } from './user-badge';
import { NavbarComponent } from './navbar';

@NgModule({
  imports: [
    BrowserModule,
  ],
  declarations: [
    UserBadgeComponent,
    NavbarComponent,
  ],
})
export class CommonModule {}
