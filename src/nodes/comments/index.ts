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
      'toastr',
      hgNodeCommentsController
    ];
  }
}

export class hgNodeCommentsController {
  hgNode: any;
  comments: any;
  constructor(
    private Comments: any,
    private toastr: any) {}

  $onInit() {
    this.getComments();
  }

  saveComment() {
    this.Comments
      .save(this.hgNode.id, this.text)
      .then(_ => {
        this.toastr.success('Your comment has been saved');
        this.getComments();
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
    ]
  }
}