import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export abstract class RemoteServiceBase {
  protected serviceName = '';

  protected constructor(protected httpClient: HttpClient) {}

  protected get apiEndpoint(): string {
    return environment.apiEndpoint;
  }

  protected logInfo(message: string) {
    console.log(`[${this.serviceName}]: ${message}`);
  }

  protected logWarn(message: string) {
    console.warn(`[${this.serviceName}]: ${message}`);
  }

  protected logError(message: string) {
    console.error(`[${this.serviceName}]: ${message}`);
  }
}
