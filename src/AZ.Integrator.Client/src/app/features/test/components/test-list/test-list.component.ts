import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { nameof } from '../../../../shared/helpers/name-of.helper';
import { TestModel } from '../../models/test.model';
import { TestState } from '../../states/test.state';
import { ChangePage, Load } from '../../states/test.action';

@Component({
  selector: 'app-test-list',
  templateUrl: './test-list.component.html',
  styleUrls: ['./test-list.component.scss'],
})
export class TestListComponent implements OnInit {
  displayedColumns: string[] = [
    nameof<TestModel>('timestamp'),
    nameof<TestModel>('type'),
    nameof<TestModel>('eventInfo'),
  ];
  logs$ = this.store.select(TestState.getLogs);
  totalItems$ = this.store.select(TestState.getLogsCount);
  currentPage$ = this.store.select(TestState.getCurrentPage);
  pageSize$ = this.store.select(TestState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new Load());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }
}
