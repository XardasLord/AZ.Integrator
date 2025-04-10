import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { RemoteServiceBase } from './remote-service.base';

@Injectable()
export class DictionaryService extends RemoteServiceBase {
  private apiUrl = environment.apiEndpoint;

  constructor() {
    const httpClient = inject(HttpClient);

    super(httpClient);
  }
}
