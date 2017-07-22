import 'rxjs/add/operator/toPromise';

import { Ng2StateDeclaration } from 'ui-router-ng2';
import { Transition } from 'ui-router-ng2';

import Nodes from 'app/api/nodes';
import Navbar from 'app/common/navbar';
import NodeList from './node-list';
import Node from './node';


export let NodesStates: Ng2StateDeclaration[] = [
  {
    url: '/nodes',
    name: 'app.hale.nodes',
    views: {
      'main@': {
        component: NodeList
      }
    }
  },
  {
    url: '/node/:nodeId',
    name: 'app.hale.node',
    views: {
      'main@': {
        component: Node,
        bindings: { node: 'node' }
      }
    },
    resolve: [
      {
        token: 'node',
        deps: [Transition, Nodes],
        resolveFn: (trans, nodes) => {
          const nodeId = trans.params().nodeId;
          return nodes
            .get(nodeId)
            .toPromise();
        }
      }
    ]
  }
];
