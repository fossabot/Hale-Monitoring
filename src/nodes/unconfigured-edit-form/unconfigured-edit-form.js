const template = require('./unconfigured-edit-form.html');

angular.module('hale.nodes')
    .component('unconfiguredEditForm', {
        bindings: {
            'hgNode': '='
        },
        templateUrl: template,
        controller: UnconfiguredEditFormController
    });

function UnconfiguredEditFormController() {
    const vm = this;
}