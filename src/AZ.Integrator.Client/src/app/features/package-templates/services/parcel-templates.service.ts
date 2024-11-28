import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RemoteServiceBase } from 'src/app/shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';
import { SaveParcelTemplateCommand } from '../models/commands/save-parcel-template.command';
import { GetOfferSignaturesResponse } from '../states/parcel-templates.state.model';

@Injectable()
export class ParcelTemplatesService extends RemoteServiceBase {
  private apiUrl = environment.apiEndpoint;

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  loadProductTags(pageInfo: PageEvent, searchText: string): Observable<GetOfferSignaturesResponse> {
    let params = new HttpParams().set('take', pageInfo.pageSize).set('skip', pageInfo.pageIndex * pageInfo.pageSize);

    if (searchText?.length > 0) {
      params = params.set('searchText', searchText);
    }

    return this.httpClient.get<GetOfferSignaturesResponse>(`${this.apiUrl}/orders/tags`, { params });
  }

  saveTemplate(command: SaveParcelTemplateCommand): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/parcelTemplates/${command.tag}`, command);
  }
}
