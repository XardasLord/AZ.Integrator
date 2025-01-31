import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RemoteServiceBase } from 'src/app/shared/services/remote-service.base';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { IntegratorQueryStocksArgs, StockViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { GraphQLHelper } from '../../../shared/graphql/graphql.helper';
import { GetStocksGQL } from '../graphql-queries/get-stocks.graphql.query';
import { ChangeStockQuantityCommand } from '../models/change-stock-quantity.command';

@Injectable()
export class StocksService extends RemoteServiceBase {
  private getStocksGql = inject(GetStocksGQL);

  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);

    super(httpClient);
  }

  getStocks(filters: IntegratorQueryStocksArgs): Observable<GraphQLResponseWithoutPaginationVo<StockViewModel[]>> {
    return this.getStocksGql
      .watch(filters, GraphQLHelper.defaultGraphQLWatchQueryOptions)
      .valueChanges.pipe(map(x => x.data));
  }

  updateStockQuantity(barcode: string, changeQuantity: number): Observable<void> {
    const command: ChangeStockQuantityCommand = {
      packageCode: barcode,
      changeQuantity: changeQuantity,
    };

    return this.httpClient.put<void>(`${this.apiUrl}/stocks/${barcode}`, command);
  }
}
