import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { filter, map, Observable } from 'rxjs';
import { RemoteServiceBase } from 'src/app/shared/services/remote-service.base';
import { environment } from '../../../../../environments/environment';
import { GraphQLResponse } from '../../../../shared/graphql/graphql.response';
import { GetPartDefinitionOrdersGQL } from '../graphql-queries/get-part-definition-orders.graphql.query';
import { PartDefinitionsOrderViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { CreateOrderRequest } from '../models/part-line-form-group.model';

@Injectable()
export class OrdersService extends RemoteServiceBase {
  private getPartDefinitionOrdersGql = inject(GetPartDefinitionOrdersGQL);

  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);
    super(httpClient);
  }

  loadOrders(filters: Record<string, unknown>): Observable<GraphQLResponse<PartDefinitionsOrderViewModel[]>> {
    return this.getPartDefinitionOrdersGql.watch({ variables: filters }).valueChanges.pipe(
      filter(result => !result.loading && result.data !== undefined),
      map(x => x.data as GraphQLResponse<PartDefinitionsOrderViewModel[]>)
    );
  }

  createOrder(command: CreateOrderRequest): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/procurement/orders`, command);
  }
}
