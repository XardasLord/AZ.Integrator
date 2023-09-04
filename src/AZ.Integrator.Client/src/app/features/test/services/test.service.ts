import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { RemoteServiceBase } from '../../../shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';
import { GetTestResponseModel } from '../models/get-test-response.model';

@Injectable()
export class TestService extends RemoteServiceBase {
  private apiUrl = environment.apiEndpoint;

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  load(pageInfo: PageEvent): Observable<GetTestResponseModel> {
    // const params = new HttpParams()
    //   .set('offset', pageInfo.pageIndex * pageInfo.pageSize)
    //   .set('count', pageInfo.pageSize);

    const logs: GetTestResponseModel = {
      logCount: 2,
      logs: [
        {
          timestamp: new Date(),
          type: 'test',
          eventInfo: 'Test Description',
        },
        {
          timestamp: new Date(),
          type: 'test 2',
          eventInfo: 'Test Description 2',
        },
      ],
    };

    return of(logs);

    // return this.httpClient.get<GetTestResponseModel>(`${this.apiUrl}/test/getLogs`, { params });
  }
}
