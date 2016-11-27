const template = require('./unconfigured-edit-form.html');

angular.module('hale.nodes')
    .component('unconfiguredEditForm', {
        bindings: {
            'hgNode': '=',
            'hgCallbacks': '='
        },
        templateUrl: template,
        controller: UnconfiguredEditFormController
    });

function UnconfiguredEditFormController() {
    const vm = this;

    vm.doSave = doSave;
    vm.doCancel = doCancel;
    vm.doBan = doBan;

    function _activate() {
        vm.node = angular.copy(vm.hgNode);
    }

    function doSave() {
        vm.hgCallbacks.save(vm.node);
    }

    function doCancel() {
        vm.hgCallbacks.cancel(vm.node);
    }

    function doBan() {
        vm.hgCallbacks.ban(vm.node);
    }

    _activate();
    

}