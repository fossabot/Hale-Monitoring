import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { UIRouterModule } from '@uirouter/angular';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { ConfigStates } from './routes';
import { ConfigComponent } from './config';
import { ConfigEditorComponent } from './config-editor';
import { ConfigListComponent } from './config-list';
import { CommonModule } from 'app/common';
import { FormsModule } from '@angular/forms';
import { AceEditorModule } from 'ng2-ace-editor';

@NgModule({
  imports: [
    AceEditorModule,
    BrowserModule,
    CommonModule,
    FormsModule,
    UIRouterModule.forChild({
      states: ConfigStates
    }),
    NgbModule,
  ],
  declarations: [
    ConfigComponent,
    ConfigEditorComponent,
    ConfigListComponent,
  ]
})
export class ConfigsModule {}
