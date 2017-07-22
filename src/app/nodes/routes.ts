import 'rxjs/add/operator/toPromise';

import { Ng2StateDeclaration } from 'ui-router-ng2';
import { Transition } from 'ui-router-ng2';

import { Nodes } from 'app/api/nodes';
import { NavbarComponent } from 'app/common/navbar';
import { NodeListComponent } from './node-list';
import { NodeComponent } from './node';

export let NodesStates: Ng2StateDeclaration[] = [
  {
    url: '/nodes',
    name: 'app.hale.nodes',
    views: {
      'main@': {
        component: NodeListComponent
      }
    }
  },
  {
    url: '/node/:nodeId',
    name: 'app.hale.node',
    views: {
      'main@': {
        component: NodeComponent,
        bindings: { node: 'node' }
      }
    },
    resolve: [
      {
        token: 'node',
        deps: [Transition, Nodes],
        resolveFn: resolveNode
      }
    ]
  }
];

export function resolveNode(trans, nodes) {
  const nodeId = trans.params().nodeId;
  return nodes
    .get(nodeId)
    .toPromise();
}
