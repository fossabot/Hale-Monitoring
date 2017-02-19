import * as angular from 'angular';

import { UnconfiguredEditFormComponent } from './unconfigured-edit-form';
import { UnconfiguredNodesListComponent} from './unconfigured-nodes-list';
import { MonitoredNodesListComponent } from './monitored-nodes-list';
import { NodeComponent } from './node';

import hgNodeBasicInfo from './basic-information';
import hgNodeTimestamps from './timestamps';
import hgNodeComments from './comments';
import NodeConstants from './constants';

export default angular
  .module('hale.nodes', [])
  .component('unconfiguredNodesList', new UnconfiguredNodesListComponent())
  .component('unconfiguredEditForm', new UnconfiguredEditFormComponent())
  .component('monitoredNodesList', new MonitoredNodesListComponent())
  .component('node', new NodeComponent())
  .component('hgNodeBasicInfo', new hgNodeBasicInfo())
  .component('hgNodeTimestamps', new hgNodeTimestamps())
  .component('hgNodeComments', new hgNodeComments())
  .constant('NodeConstants', NodeConstants)
  .name;
