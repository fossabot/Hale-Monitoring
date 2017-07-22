import { NgModule } from '@angular/core';
import { UIRouterModule} from 'ui-router-ng2';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NodesStates } from './routes';

import { NodeComponent } from './node';
import { NodeBasicsComponent } from './node-basics';
import { NodeTimestampsComponent } from './node-timestamps';
import { NodeListComponent } from './node-list';
import { NodeCommentsComponent } from './node-comments';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    UIRouterModule.forChild({
      states: NodesStates
    })
  ],
  declarations: [
    NodeComponent,
    NodeBasicsComponent,
    NodeTimestampsComponent,
    NodeListComponent,
    NodeCommentsComponent,
  ],
})

export class NodesModule {}
