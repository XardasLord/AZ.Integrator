import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { RemoteServiceBase } from 'src/app/shared/services/remote-service.base';
import { environment } from '../../../../../environments/environment';
import { GraphQLResponse } from '../../../../shared/graphql/graphql.response';
import { GetSuppliersGQL } from '../graphql-queries/get-suppliers.graphql.query';
import { GraphQLHelper } from '../../../../shared/graphql/graphql.helper';
import { SaveSupplierCommand } from '../models/commands/save-supplier.command';
import { SupplierViewModel } from '../../../../shared/graphql/graphql-integrator.schema';

@Injectable()
export class SuppliersService extends RemoteServiceBase {
  private getSuppliersGql = inject(GetSuppliersGQL);

  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);
    super(httpClient);
  }

  loadSuppliers(filters: Record<string, unknown>): Observable<GraphQLResponse<SupplierViewModel[]>> {
    return this.getSuppliersGql
      .watch(filters, GraphQLHelper.defaultGraphQLWatchQueryOptions)
      .valueChanges.pipe(map(x => x.data));
  }

  addSupplier(command: SaveSupplierCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/procurement/suppliers`, command);
  }

  updateSupplier(command: SaveSupplierCommand): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/procurement/suppliers/${command.id}`, command);
  }

  deleteSupplier(supplierId: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/procurement/suppliers/${supplierId}`);
  }
}
