import * as angular from 'angular';

import { UnconfiguredEditFormComponent } from './unconfigured-edit-form';
import { UnconfiguredNodesListComponent} from './unconfigured-nodes-list';
import { MonitoredNodesListComponent } from './monitored-nodes-list';
import { NodeComponent } from './node';

import NodeConstants from './constants';

export default angular
  .module('hale.nodes', [])
  .component('unconfiguredNodesList', new UnconfiguredNodesListComponent())
  .component('unconfiguredEditForm', new UnconfiguredEditFormComponent())
  .component('monitoredNodesList', new MonitoredNodesListComponent())
  .component('node', new NodeComponent())
  .constant('NodeConstants', NodeConstants)
  .name;
