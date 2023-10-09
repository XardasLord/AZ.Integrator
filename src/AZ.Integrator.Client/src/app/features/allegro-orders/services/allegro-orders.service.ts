import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { RemoteServiceBase } from '../../../shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';
import { GetAllegroOrdersResponseModel } from '../models/get-allegro-orders-response.model';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';
import { CreateInpostShipmentCommand } from '../models/commands/create-inpost-shipment.command';
import { GraphQLHelper } from '../../../shared/graphql/graphql.helper';
import {
  InpostShipmentViewModel,
  IntegratorQueryInpostShipmentsArgs,
} from '../../../shared/graphql/graphql-integrator.schema';
import { GetInpostShipmentsGQL } from '../graphql-queries/get-inpost-shipments.graphql.query';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { AllegroOrderFulfillmentStatusEnum } from '../models/allegro-order-fulfillment-status.enum';

@Injectable()
export class AllegroOrdersService extends RemoteServiceBase {
  private apiUrl = environment.apiEndpoint;

  constructor(
    httpClient: HttpClient,
    private inpostShipmentsGqlQuery: GetInpostShipmentsGQL
  ) {
    super(httpClient);
  }

  load(
    pageInfo: PageEvent,
    orderFulfillmentStatus: AllegroOrderFulfillmentStatusEnum
  ): Observable<GetAllegroOrdersResponseModel> {
    const params = new HttpParams()
      .set('orderFulfillmentStatus', orderFulfillmentStatus)
      .set('take', pageInfo.pageSize)
      .set('skip', pageInfo.pageIndex * pageInfo.pageSize);

    return this.httpClient.get<GetAllegroOrdersResponseModel>(`${this.apiUrl}/allegroOrders`, { params });
  }

  getDetails(orderId: string): Observable<AllegroOrderDetailsModel> {
    return this.httpClient.get<AllegroOrderDetailsModel>(`${this.apiUrl}/allegroOrders/${orderId}`);
  }

  // TODO: Move it to a new dedicated shipment service
  registerInpostShipment(shipment: CreateInpostShipmentCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/inpostShipments/`, shipment);
  }

  getInpostShipments(
    filters: IntegratorQueryInpostShipmentsArgs
  ): Observable<GraphQLResponseWithoutPaginationVo<InpostShipmentViewModel[]>> {
    return this.inpostShipmentsGqlQuery
      .watch(filters, GraphQLHelper.defaultGraphQLWatchQueryOptions)
      .valueChanges.pipe(map(x => x.data));
  }
}
