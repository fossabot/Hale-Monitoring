
import { NgModule } from '@angular/core';
import { UIRouterModule } from '@uirouter/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AdminRoutes } from './routes';
import { AdminComponent } from './admin.component';
import { AdminMenuComponent } from './admin-menu';
import { UserManagementComponent } from './user-management';
import { CreateUserComponent } from './create-user';
import { EditUserComponent } from './edit-user';
import { CommonModule } from 'app/common';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    CommonModule,
    NgbModule,
    UIRouterModule.forChild({
      states: AdminRoutes,
    })
  ],
  declarations: [
    AdminComponent,
    AdminMenuComponent,
    CreateUserComponent,
    EditUserComponent,
    UserManagementComponent,
  ],
})
export class AdminModule {}
