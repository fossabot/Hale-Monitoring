import { Component, Input, Output, EventEmitter } from '@angular/core';
import { AceEditorDirective } from 'ng2-ace-editor';

@Component({
  selector: 'app-config-editor',
  template: `<div ace-editor
    [(text)]="text"
    [mode]="'yaml'"
    [theme]="'chrome'"
    [options]="options"
    [autoUpdateContent]="true"
    style=" width:100%; overflow: auto;margin-top:10px"></div>`
})
export class ConfigEditorComponent {
  @Input() config: string;
  @Output() configChange: EventEmitter<string> = new EventEmitter<string>();

  get text() {
    return this.config;
  }

  set text(val) {
    this.config = val;
    this.configChange.emit(this.config);
  }

  options: {[key: string]: any} = {
    printMargin: false,
    showInvisibles: true,
    maxLines: 100,
  };
}
