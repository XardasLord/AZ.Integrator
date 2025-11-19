import { inject, Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { HttpClient, HttpParams } from '@angular/common/http';
import { filter, map, Observable } from 'rxjs';
import { RemoteServiceBase } from '../../../shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';
import { GetOrdersResponseModel } from '../models/get-orders-response.model';
import { OrderDetailsModel } from '../models/order-details.model';
import { CreateShipmentCommand } from '../models/commands/create-shipment.command';
import { IntegratorQueryShipmentsArgs, ShipmentViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { OrderFulfillmentStatusEnum } from '../models/order-fulfillment-status.enum';
import { GetShipmentsGQL } from '../graphql-queries/get-shipments.graphql.query';
import { GenerateInvoiceCommand } from '../models/commands/generate-invoice.command';

@Injectable()
export class OrdersService extends RemoteServiceBase {
  private shipmentsGqlQuery = inject(GetShipmentsGQL);

  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);

    super(httpClient);
  }

  load(
    pageInfo: PageEvent,
    orderFulfillmentStatuses: OrderFulfillmentStatusEnum[],
    searchText: string
  ): Observable<GetOrdersResponseModel> {
    let params = new HttpParams().set('take', pageInfo.pageSize).set('skip', pageInfo.pageIndex * pageInfo.pageSize);

    orderFulfillmentStatuses.forEach(status => {
      params = params.append('orderFulfillmentStatus', status);
    });

    if (searchText?.length > 0) {
      params = params.set('searchText', searchText);
    }

    return this.httpClient.get<GetOrdersResponseModel>(`${this.apiUrl}/orders`, { params });
  }

  getDetails(orderId: string): Observable<OrderDetailsModel> {
    return this.httpClient.get<OrderDetailsModel>(`${this.apiUrl}/orders/${orderId}`);
  }

  // TODO: Move it to a new dedicated shipment service
  registerInpostShipment(shipment: CreateShipmentCommand): Observable<ShipmentViewModel> {
    return this.httpClient.post<ShipmentViewModel>(`${this.apiUrl}/inpostShipments/`, shipment);
  }

  registerDpdShipment(shipment: CreateShipmentCommand): Observable<ShipmentViewModel> {
    return this.httpClient.post<ShipmentViewModel>(`${this.apiUrl}/dpdShipments/`, shipment);
  }

  getShipments(
    filters: IntegratorQueryShipmentsArgs
  ): Observable<GraphQLResponseWithoutPaginationVo<ShipmentViewModel[]>> {
    return this.shipmentsGqlQuery.watch({ variables: filters }).valueChanges.pipe(
      filter(result => !result.loading && result.data !== undefined),
      map(x => x.data as GraphQLResponseWithoutPaginationVo<ShipmentViewModel[]>)
    );
  }

  // TODO: Move it to a new dedicated invoice service
  generateInvoice(command: GenerateInvoiceCommand): Observable<string> {
    return this.httpClient.post<string>(`${this.apiUrl}/invoices`, command);
  }
}
