import { NgModule } from '@angular/core';
import { UIRouterModule} from 'ui-router-ng2';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NodesStates } from './routes';

import Node from './node';
import NodeBasics from './node-basics';
import NodeTimestamps from './node-timestamps';
import NodeList from './node-list';
import NodeComments from './node-comments';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    UIRouterModule.forChild({
      states: NodesStates
    })
  ],
  declarations: [
    Node,
    NodeBasics,
    NodeTimestamps,
    NodeList,
    NodeComments,
  ],
})

export default class NodesModule {}
