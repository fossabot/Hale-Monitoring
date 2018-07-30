import * as angular from 'angular';
import { IComponentOptions } from 'angular';

const template = require('./unconfigured-edit-form.html');

export class UnconfiguredEditFormComponent implements IComponentOptions {
    bindings: {[key: string]: string};
    controller: any;
    templateUrl: any;

    constructor() {
        this.templateUrl = template;
        this.controller = UnconfiguredEditFormController;
        this.bindings = {
            hgNode: '=',
            hgCallbacks: '='
        };
    }

}


export class UnconfiguredEditFormController {
    hgNode: any;
    hgCallbacks: any;
    node: any;

    $onInit() {
        this.node = angular.copy(this.hgNode);
    }

    doSave() {
        this.hgCallbacks.save(this.node);
    }

    doCancel() {
        this.hgCallbacks.cancel(this.node);
    }

    doBan() {
        this.hgCallbacks.ban(this.node);
    }
}