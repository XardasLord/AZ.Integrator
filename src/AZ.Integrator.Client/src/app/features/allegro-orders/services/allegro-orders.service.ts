import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RemoteServiceBase } from '../../../shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';
import { GetAllegroOrdersResponseModel } from '../models/get-allegro-orders-response.model';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';
import { CreateInpostShipmentCommand } from '../models/commands/create-inpost-shipment.command';

@Injectable()
export class AllegroOrdersService extends RemoteServiceBase {
  private apiUrl = environment.apiEndpoint;

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  load(pageInfo: PageEvent): Observable<GetAllegroOrdersResponseModel> {
    return this.httpClient.get<GetAllegroOrdersResponseModel>(`${this.apiUrl}/allegroOrders`);
  }

  getDetails(orderId: string): Observable<AllegroOrderDetailsModel> {
    return this.httpClient.get<AllegroOrderDetailsModel>(`${this.apiUrl}/allegroOrders/${orderId}`);
  }

  // TODO: Move it to a new dedicated shipment service
  registerInpostShipment(shipment: CreateInpostShipmentCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/inpostShipments/`, shipment);
  }
}
