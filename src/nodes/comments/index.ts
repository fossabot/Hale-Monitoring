import { IComponentOptions } from 'angular';

const template = require('./comments.html');

export default class hgNodeComments implements IComponentOptions {
  bindings: {[key: string]: string};
  templateUrl: any;
  controller: any;

  constructor() {
    this.templateUrl = template;
    this.bindings = {
      hgNode: '='
    }
    this.controller = [
      'Comments',
      'Users',
      'toastr',
      hgNodeCommentsController
    ];
  }
}

export class hgNodeCommentsController {
  hgNode: any;
  comments: any;

  text: any;
  user: any;


  constructor(
    private Comments: any,
    private Users: any,
    private toastr: any) {}

  $onInit() {
    this.getComments();
    this.getUser();
  }

  saveComment() {
    this.Comments
      .save(this.hgNode.id, this.text)
      .then(_ => {
        this.toastr.success('Your comment has been saved');
        this.getComments();
        this.text = '';
      });
  }

  deleteComment(id: number) {
    this.Comments
      .delete(this.hgNode.id, id)
      .then(_ => {
        this.toastr.success('The comment has been deleted');        
        this.getComments();
      });
  }

  private getComments() {
    this.Comments
      .get(this.hgNode.id)
      .then((data) => {
        this.comments = data;
      });
  }

  private getUser() {
    this.Users
      .getCurrent()
      .then((user: any) => {
        this.user = user;
      });

  }
}