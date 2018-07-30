import { NgModule } from '@angular/core';
import { UIRouterModule} from '@uirouter/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NodesStates } from './routes';


import { CommonModule } from 'app/common';
import { NodeComponent } from './node';
import { NodeBasicsComponent } from './node-basics';
import { NodeTimestampsComponent } from './node-timestamps';
import { NodeListComponent } from './node-list';
import { NodeCommentsComponent } from './node-comments';
import { NodeChecksComponent } from './node-checks';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    CommonModule,
    UIRouterModule.forChild({
       states: NodesStates
    })
  ],
  declarations: [
    NodeComponent,
    NodeChecksComponent,
    NodeBasicsComponent,
    NodeTimestampsComponent,
    NodeListComponent,
    NodeCommentsComponent,
  ],
})

export class NodesModule {}
