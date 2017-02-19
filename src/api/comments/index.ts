import { IHttpService } from 'angular';

export class CommentsService {
  baseUrl: string;

  constructor(private $http: IHttpService) {
    this.baseUrl = 'http://localhost:8989/api/v1/hosts';
  }

  get(nodeId: number) {
    return this.$http({
      method: 'GET',
      url: `${this.baseUrl}/${nodeId}/comments`,
      withCredentials: true
    }).then((response: any) => response.data)
    .catch(() => undefined);
  }

  save(nodeId: number, text: string) {
    return this.$http({
      method: 'POST',
      url: `${this.baseUrl}/${nodeId}/comments`,
      data: { text: text },
      withCredentials: true
    });
  }

  delete(nodeId: number, commentId: number) {
    return this.$http({
      method: 'DELETE',
      url: `${this.baseUrl}/${nodeId}/comments/${commentId}`,
      withCredentials: true      
    });
  }
} 