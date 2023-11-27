import { PageEvent } from '@angular/material/paginator';
import { RestQueryHelper } from './rest.helper';

export class RestQueryVo {
  public currentPage: PageEvent = RestQueryHelper.getInitialPageEvent();
  public searchText: string = '';

  constructor(init?: Partial<RestQueryVo>) {
    Object.assign(this, { ...init });
  }
}
