import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Comments } from 'app/api/comments';
import { Users } from 'app/api/users';

@Component({
  selector: 'app-node-comments',
  templateUrl: './node-comments.html',
  styleUrls: [ './node-comments.scss']
})
export class NodeCommentsComponent implements OnInit {
  @Input() nodeId: number;

  comments: any;
  text: string;
  user: any;

  constructor(
    private Comments: Comments,
    private Users: Users) {}

  ngOnInit() {
    this.getComments();
    this.getUser();
  }

  private getComments() {
    this.Comments
      .get(this.nodeId)
      .subscribe((comments) => {
        this.comments = comments;
      });
  }

  private getUser() {
    this.Users
      .getCurrent()
      .subscribe((user: any) => {
        this.user = user;
      });
  }


  saveComment(form: NgForm) {
    this.Comments
      .save(this.nodeId, form.value.text)
      .subscribe(
        () => {
          this.handleSaveSuccess();
          form.reset();
        },
        () => this.handleSaveError(),
       );

  }

  private handleSaveSuccess(): void {
    // TODO: Show toastr -SA 2017-07-22
    this.getComments();
    this.text = '';
  }

  private handleSaveError(): void {
    // TODO: Show toastr -SA 2017-07-22
  }

  deleteComment(id: number) {
    this.Comments
      .delete(this.nodeId, id)
      .subscribe(
        _ => this.handleDeleteSuccess(),
        _ => this.handleDeleteError()
      );
  }

  private handleDeleteSuccess(): void {
    // TODO: Show toastr -SA 2017-07-22
    this.getComments();
  }

  private handleDeleteError(): void {
    // TODO: Show toastr -SA 2017-07-22
  }

  getGravatarUrl(email: string): string {
    return this.Users.getGravatarUrl(email);
  }

}
