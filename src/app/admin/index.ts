import { NgModule } from '@angular/core';
import { UIRouterModule } from '@uirouter/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AdminComponent } from './admin.component';
import { AdminMenuComponent } from './admin-menu';
import { ChangePasswordComponent } from './change-password';
import { EditProfileComponent } from './edit-profile';

@NgModule({
  declarations: [
    AdminComponent,
    AdminMenuComponent,
    ChangePasswordComponent,
    EditProfileComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    UIRouterModule.forChild({
      states: [
        {
          url: '/admin',
          name: 'app.hale.admin',

          views: {
            'main@': {
              component: AdminComponent
            },
            'adminMenu@app.hale.admin': {
              component: AdminMenuComponent
            }
          }
        },
        {
          url: '/edit-profile',
          name: 'app.hale.admin.edit-profile',
          views: {
            'adminContent@app.hale.admin': {
              component: EditProfileComponent
            }
          }
        },
        {
          url: '/change-password',
          name: 'app.hale.admin.change-password',
          views: {
            'adminContent@app.hale.admin': {
              component: ChangePasswordComponent
            }
          }
        }
      ]
    })
  ],
  providers: []
})
export class AdminModule {}
