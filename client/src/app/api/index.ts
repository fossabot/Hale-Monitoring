import { NgModule } from '@angular/core';

import { Auth } from './auth';
import { Comments } from './comments';
import { Configs } from './configs';
import { Nodes } from './nodes';
import { Users } from './users';
import { Results } from './results';

@NgModule({
  imports: [],
  providers: [
    Auth,
    Comments,
    Nodes,
    Users,
    Configs,
    Results,
  ],
  declarations: [
  ]
})
export class ApiModule {}
