import { PageEvent } from '@angular/material/paginator';

export class RestQueryHelper {
  public static getInitialPageEvent(pageSize = 10): PageEvent {
    const output = new PageEvent();

    output.pageIndex = 0;
    output.pageSize = pageSize;
    output.previousPageIndex = undefined;

    return output;
  }
}
