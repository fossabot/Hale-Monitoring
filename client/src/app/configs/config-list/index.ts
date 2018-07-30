import { Component } from '@angular/core';
import { Configs } from 'app/api/configs';
import { Users } from 'app/api/users';

@Component({
  selector: 'app-config-list',
  templateUrl: './config-list.html',
  styleUrls: [
    './config-list.scss'
  ]
})
export class ConfigListComponent {

  configs: any[];
  devIsCollapsed: boolean = true;

  constructor(private Configs: Configs, private Users: Users) {
    this.Configs
      .list()
      .subscribe(list => {
        this.configs = list;
      });
  }

  getContentString(config: any): string {
    const parts = [];

    if (config.checkCount > 0) {
      parts.push(this.generateContentPart(config.checkCount, 'check'));
    }
    if (config.actionCount > 0) {
      parts.push(this.generateContentPart(config.actionCount, 'action'));
    }
    if (config.infoCount > 0) {
      parts.push(`${config.infoCount} info`);
    }
    return `${parts.join(', ')}`;
  }

   getGravatarUrl(email: string) {
    return this.Users.getGravatarUrl(email);
  }

  private generateContentPart(count: number, singularizedWord: string) {
    return `${count} ${singularizedWord}${count > 1 ? 's' : ''}`;
  }
}
