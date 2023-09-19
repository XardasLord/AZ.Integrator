import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { TestModel } from '../../../test/models/test.model';
import { AllegroOrdersState } from '../../states/allegro-orders.state';
import { ChangePage, Load } from '../../states/allegro-orders.action';

@Component({
  selector: 'app-allegro-orders-list',
  templateUrl: './allegro-orders-list.component.html',
  styleUrls: ['./allegro-orders-list.component.scss'],
})
export class AllegroOrdersListComponent implements OnInit {
  displayedColumns: string[] = [
    nameof<TestModel>('timestamp'),
    nameof<TestModel>('type'),
    nameof<TestModel>('eventInfo'),
  ];
  orders$ = this.store.select(AllegroOrdersState.getLogs);
  totalItems$ = this.store.select(AllegroOrdersState.getLogsCount);
  currentPage$ = this.store.select(AllegroOrdersState.getCurrentPage);
  pageSize$ = this.store.select(AllegroOrdersState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new Load());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }
}
