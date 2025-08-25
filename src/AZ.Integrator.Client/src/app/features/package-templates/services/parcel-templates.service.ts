import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { RemoteServiceBase } from 'src/app/shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';
import { SaveParcelTemplateCommand } from '../models/commands/save-parcel-template.command';
import {
  IntegratorQueryTagParcelTemplatesArgs,
  TagParcelTemplateViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLResponse } from '../../../shared/graphql/graphql.response';
import { GetTagParcelTemplatesGQL } from '../graphql-queries/get-tag-parcel-templates.graphql.query';
import { GraphQLHelper } from '../../../shared/graphql/graphql.helper';

@Injectable()
export class ParcelTemplatesService extends RemoteServiceBase {
  private getTagParcelTemplatesGql = inject(GetTagParcelTemplatesGQL);

  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);

    super(httpClient);
  }

  loadTemplates(
    filters: IntegratorQueryTagParcelTemplatesArgs
  ): Observable<GraphQLResponse<TagParcelTemplateViewModel[]>> {
    return this.getTagParcelTemplatesGql
      .watch(filters, GraphQLHelper.defaultGraphQLWatchQueryOptions)
      .valueChanges.pipe(map(x => x.data));
  }

  saveTemplate(command: SaveParcelTemplateCommand): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/parcelTemplates/${command.tag}`, command);
  }
}
