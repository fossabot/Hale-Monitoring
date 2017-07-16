import * as angular from 'angular';

import { AuthService } from './auth';
import { NodesService } from './nodes';
import { CommentsService } from './comments';
import { UsersService } from './users';
export default angular
  .module('hale.api', [])
  .service('Auth', AuthService)
  .service('Nodes', NodesService)
  .service('Comments', CommentsService)
  .service('Users', UsersService)
  .name;