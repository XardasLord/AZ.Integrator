import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';


@Component({
  selector: 'app-scroll-table',
  imports: [MatPaginator],
  templateUrl: './scroll-table.component.html',
  styleUrls: ['./scroll-table.component.scss'],
  standalone: true,
})
export class ScrollTableComponent {
  @Input() totalItems!: number;
  @Input() currentPage!: number;
  @Input() pageSize!: number;
  @Output() page = new EventEmitter<PageEvent>();

  pageChanged(event: PageEvent): void {
    this.page.emit(event);
  }
}
