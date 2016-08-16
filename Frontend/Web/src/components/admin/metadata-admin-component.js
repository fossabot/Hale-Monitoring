(function() {
angular.module('hale.gui')
  .component('metadataAdmin', {
    templateUrl: './views/admin/metadata.html',
    controller: 'MetadataAdminController'
  })
  .controller('MetadataAdminController', function() {
    var vm = this;
    vm.title =
      'Metadata Templates';
    vm.description =
      'Metadata is additional attributes you may define to extend ' +
      'the standard data forms so that they correspond to your organizations ' +
      'reality. Each metadata attribute is tied to a certain template that ' +
      'will also be the context in which it wil be rendered and/or edited.';
  });
})();
