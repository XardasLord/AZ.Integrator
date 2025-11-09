import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RemoteServiceBase } from 'src/app/shared/services/remote-service.base';
import { filter, map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { IntegratorQueryStockGroupsArgs, StockGroupViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { GetStockGroupsGQL } from '../graphql-queries/get-stock-groups.graphql.query';
import { UpdateStockGroupCommand } from '../models/update-stock-group.command';
import { AddStockGroupCommand } from '../models/add-stock-group.command';

@Injectable()
export class StockGroupsService extends RemoteServiceBase {
  private getStockGroupsGql = inject(GetStockGroupsGQL);

  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);

    super(httpClient);
  }

  getStockGroups(
    filters: IntegratorQueryStockGroupsArgs
  ): Observable<GraphQLResponseWithoutPaginationVo<StockGroupViewModel[]>> {
    return this.getStockGroupsGql.watch({ variables: filters }).valueChanges.pipe(
      filter(result => !result.loading && result.data !== undefined),
      map(x => x.data as GraphQLResponseWithoutPaginationVo<StockGroupViewModel[]>)
    );
  }

  add(name: string, description: string): Observable<AddStockGroupResponse> {
    const command: AddStockGroupCommand = {
      name,
      description,
    };

    return this.httpClient.post<AddStockGroupResponse>(`${this.apiUrl}/stock-groups`, command);
  }

  update(groupId: number, name: string, description: string): Observable<void> {
    const command: UpdateStockGroupCommand = {
      groupId,
      name,
      description,
    };

    return this.httpClient.put<void>(`${this.apiUrl}/stock-groups/${groupId}`, command);
  }
}

export interface AddStockGroupResponse {
  id: number;
}
