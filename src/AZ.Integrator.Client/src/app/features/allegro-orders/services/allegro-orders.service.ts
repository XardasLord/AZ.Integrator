import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
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
    return this.httpClient.get<GetAllegroOrdersResponseModel>(`${this.apiUrl}/allegroOrders`);
  }
}
