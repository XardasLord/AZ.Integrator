import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { filter, map, Observable } from 'rxjs';
import { RemoteServiceBase } from 'src/app/shared/services/remote-service.base';
import { environment } from '../../../../../environments/environment';
import { GraphQLResponse } from '../../../../shared/graphql/graphql.response';
import { GetFurnitureDefinitionsGQL } from '../graphql-queries/get-furniture-definitions.graphql.query';
import { SaveFurnitureDefinitionCommand } from '../models/commands/save-furniture-definition.command';
import { FurnitureModelViewModel } from '../../../../shared/graphql/graphql-integrator.schema';

@Injectable()
export class FurnitureFormatsService extends RemoteServiceBase {
  private getFurnitureDefinitionsGql = inject(GetFurnitureDefinitionsGQL);

  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);
    super(httpClient);
  }

  loadFurnitureDefinitions(filters: Record<string, unknown>): Observable<GraphQLResponse<FurnitureModelViewModel[]>> {
    return this.getFurnitureDefinitionsGql.watch({ variables: filters }).valueChanges.pipe(
      filter(result => !result.loading && result.data !== undefined),
      map(x => x.data as GraphQLResponse<FurnitureModelViewModel[]>)
    );
  }

  addFurnitureDefinition(command: SaveFurnitureDefinitionCommand): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/catalog/furniture-models`, command);
  }

  updateFurnitureDefinition(command: SaveFurnitureDefinitionCommand): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/catalog/furniture-models/${command.furnitureCode}`, command);
  }

  deleteFurnitureDefinition(furnitureCode: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/catalog/furniture-models/${furnitureCode}`);
  }
}
