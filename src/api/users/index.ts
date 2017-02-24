import { IPromise, IHttpService } from 'angular';

export class UsersService {
  baseUrl: string = 'http://localhost:8989/api/v1';
  $injector: any[] = [ '$http' ];

  constructor(private $http: IHttpService) {}

  getCurrent(): IPromise<any> {
    return this.$http({
      method: 'GET',
      url: `${this.baseUrl}/users/current`,
      withCredentials: true
    }).then((response: any) => response.data);
  }
}