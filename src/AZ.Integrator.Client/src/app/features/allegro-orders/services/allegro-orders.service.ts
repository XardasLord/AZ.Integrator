import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { RemoteServiceBase } from '../../../shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';
import { GetAllegroOrdersResponseModel } from '../models/get-allegro-orders-response.model';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';
import { CreateShipmentCommand } from '../models/commands/create-shipment.command';
import { GraphQLHelper } from '../../../shared/graphql/graphql.helper';
import { IntegratorQueryShipmentsArgs, ShipmentViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { AllegroOrderFulfillmentStatusEnum } from '../models/allegro-order-fulfillment-status.enum';
import { GetShipmentsGQL } from '../graphql-queries/get-shipments.graphql.query';
import { GenerateInvoiceCommand } from '../models/commands/generate-invoice.command';

@Injectable()
export class AllegroOrdersService extends RemoteServiceBase {
  private apiUrl = environment.apiEndpoint;

  constructor(
    httpClient: HttpClient,
    private shipmentsGqlQuery: GetShipmentsGQL
  ) {
    super(httpClient);
  }

  load(
    pageInfo: PageEvent,
    orderFulfillmentStatuses: AllegroOrderFulfillmentStatusEnum[],
    searchText: string
  ): Observable<GetAllegroOrdersResponseModel> {
    let params = new HttpParams().set('take', pageInfo.pageSize).set('skip', pageInfo.pageIndex * pageInfo.pageSize);

    orderFulfillmentStatuses.forEach(status => {
      params = params.append('orderFulfillmentStatus', status);
    });

    if (searchText?.length > 0) {
      params = params.set('searchText', searchText);
    }

    return this.httpClient.get<GetAllegroOrdersResponseModel>(`${this.apiUrl}/allegroOrders`, { params });
  }

  getDetails(orderId: string): Observable<AllegroOrderDetailsModel> {
    return this.httpClient.get<AllegroOrderDetailsModel>(`${this.apiUrl}/allegroOrders/${orderId}`);
  }

  // TODO: Move it to a new dedicated shipment service
  registerInpostShipment(shipment: CreateShipmentCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/inpostShipments/`, shipment);
  }

  registerDpdShipment(shipment: CreateShipmentCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/dpdShipments/`, shipment);
  }

  getShipments(
    filters: IntegratorQueryShipmentsArgs
  ): Observable<GraphQLResponseWithoutPaginationVo<ShipmentViewModel[]>> {
    return this.shipmentsGqlQuery
      .watch(filters, GraphQLHelper.defaultGraphQLWatchQueryOptions)
      .valueChanges.pipe(map(x => x.data));
  }

  // TODO: Move it to a new dedicated invoice service
  generateInvoice(command: GenerateInvoiceCommand): Observable<string> {
    return this.httpClient.post<string>(`${this.apiUrl}/invoices`, command);
  }
}
