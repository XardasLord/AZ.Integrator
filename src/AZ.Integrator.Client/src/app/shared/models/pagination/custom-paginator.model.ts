import { MatPaginatorIntl } from '@angular/material/paginator';

export function CustomPaginator() {
  const customPaginatorInitial = new MatPaginatorIntl();

  customPaginatorInitial.itemsPerPageLabel = 'Elementów na stronę:';

  return customPaginatorInitial;
}
