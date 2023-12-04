import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RemoteServiceBase } from 'src/app/shared/services/remote-service.base';
import { environment } from '../../../../environments/environment';

@Injectable()
export class PackageTemplatesService extends RemoteServiceBase {
  private apiUrl = environment.apiEndpoint;

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  loadProductTags(pageInfo: PageEvent, searchText: string): Observable<string[]> {
    let params = new HttpParams().set('take', pageInfo.pageSize).set('skip', pageInfo.pageIndex * pageInfo.pageSize);

    if (searchText?.length > 0) {
      params = params.set('searchText', searchText);
    }

    return this.httpClient.get<string[]>(`${this.apiUrl}/allegroOrders/tags`, { params });
  }
}
