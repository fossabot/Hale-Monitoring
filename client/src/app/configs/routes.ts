import 'rxjs/add/operator/toPromise';

import { Ng2StateDeclaration, Transition } from '@uirouter/angular';
import { ConfigComponent } from './config';
import { ConfigListComponent } from './config-list';
import { Configs } from 'app/api/configs';

export let ConfigStates: Ng2StateDeclaration[] = [
  {
    url: '/configs',
    name: 'app.hale.configs',
    views: {
      'main@': {
        component: ConfigListComponent
      }
    }
  },
  {
    url: '/configs/:id',
    name: 'app.hale.config',
    resolve: [
      {
        token: 'config',
        deps: [Transition, Configs],
        resolveFn: resolveConfig,
      },
      {
        token: 'id',
        deps: [Transition],
        resolveFn: resolveConfigId,
      }
    ],
    views: {
      'main@': {
        component: ConfigComponent
      }
    }
  }
];

export function resolveConfig(trans: Transition, configs: Configs) {
  const configId = trans.params().id;
  return configs
    .get(configId)
    .toPromise();
}

export function resolveConfigId(trans: Transition) {
  return trans.params().id;
}
