import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { RemoteServiceBase } from '../../../shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';
import { GetAllegroOrdersResponseModel } from '../models/get-allegro-orders-response.model';

@Injectable()
export class AllegroOrdersService extends RemoteServiceBase {
  private apiUrl = environment.apiEndpoint;

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  load(pageInfo: PageEvent): Observable<GetAllegroOrdersResponseModel> {
    // const params = new HttpParams()
    //   .set('offset', pageInfo.pageIndex * pageInfo.pageSize)
    //   .set('count', pageInfo.pageSize);

    const orders: GetAllegroOrdersResponseModel = {
      ordersCount: 2,
      orders: [
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

    return of(orders);

    // return this.httpClient.get<GetAllegroOrdersResponseModel>(`${this.apiUrl}/test/getLogs`, { params });
  }
}
