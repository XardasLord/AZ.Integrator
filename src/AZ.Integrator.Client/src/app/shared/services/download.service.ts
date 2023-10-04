import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RemoteServiceBase } from './remote-service.base';

@Injectable()
export class DownloadService extends RemoteServiceBase {
  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  public getFile(data: Blob, name: string): void {
    const dataType = data.type;
    const binaryData = [];
    binaryData.push(data);

    const fileURL = window.URL.createObjectURL(new Blob(binaryData, { type: dataType }));
    const tempLink = document.createElement('a');
    tempLink.href = fileURL;
    tempLink.setAttribute('download', `${name}`);
    tempLink.dispatchEvent(new MouseEvent(`click`));
  }

  public downloadFileFromApi(uri: string, httpParams?: HttpParams): Observable<Blob> {
    return this.downloadFile(this.apiEndpoint, uri, httpParams);
  }

  private downloadFile(baseUrl: string, uri: string, httpParams?: HttpParams) {
    const url = `${baseUrl}${uri}`;

    return this.httpClient.get<Blob>(url, {
      responseType: 'blob' as 'json',
      params: httpParams,
    });
  }
}
