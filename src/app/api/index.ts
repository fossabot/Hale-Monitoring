import { NgModule } from '@angular/core';

import { Auth } from './auth';
import { Comments } from './comments';
import { Nodes } from './nodes';
import { Users } from './users';

@NgModule({
  imports: [],
  providers: [
    Auth,
    Comments,
    Nodes,
    Users,
  ],
  declarations: [
  ]
})
export class ApiModule {}
