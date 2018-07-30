import { NgModule } from '@angular/core';
import { UIRouterModule } from '@uirouter/angular';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CommonModule } from 'app/common';

import { ProfileComponent } from './profile.component';
import { ProfileMenuComponent } from './profile-menu';
import { ChangePasswordComponent } from './change-password';
import { EditProfileComponent } from './edit-profile';

@NgModule({
  declarations: [
    ProfileComponent,
    ProfileMenuComponent,
    ChangePasswordComponent,
    EditProfileComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    CommonModule,
    UIRouterModule.forChild({
      states: [
        {
          url: '/profile',
          name: 'app.hale.profile',

          views: {
            'main@': {
              component: ProfileComponent
            },
            'profileMenu@app.hale.profile': {
              component: ProfileMenuComponent
            }
          }
        },
        {
          url: '/edit',
          name: 'app.hale.profile.edit',
          views: {
            'profileContent@app.hale.profile': {
              component: EditProfileComponent
            }
          }
        },
        {
          url: '/password',
          name: 'app.hale.profile.password',
          views: {
            'profileContent@app.hale.profile': {
              component: ChangePasswordComponent
            }
          }
        }
      ]
    })
  ],
  providers: []
})
export class ProfileModule {}
