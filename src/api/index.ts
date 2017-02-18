import * as angular from 'angular';

import { AuthService } from './auth';
import { NodesService } from './nodes';

export default angular
  .module('hale.api', [])
  .service('Auth', AuthService)
  .service('Nodes', NodesService)
  .name;