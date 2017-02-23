import * as angular from 'angular';

import { BadgeComponent } from './badge';
import { NavbarComponent } from './navbar';

export default angular
  .module('hale.common', [])
  .component('badge', new BadgeComponent())
  .component('navbar', new NavbarComponent())
  .name;
