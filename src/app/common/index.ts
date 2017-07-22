import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import UserBadge from './user-badge';
import Navbar from './navbar';

@NgModule({
  imports: [
    BrowserModule,
  ],
  declarations: [
    UserBadge,
    Navbar,
  ],
})
export default class CommonModule {}
