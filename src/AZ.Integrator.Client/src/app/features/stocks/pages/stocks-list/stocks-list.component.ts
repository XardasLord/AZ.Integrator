import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { Store } from '@ngxs/store';
import { LoadStocks } from '../../states/stocks.action';
import { Observable } from 'rxjs';
import { StockViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { StocksState } from '../../states/stocks.state';

@Component({
  selector: 'app-stocks-list',
  imports: [SharedModule],
  templateUrl: './stocks-list.component.html',
  styleUrl: './stocks-list.component.scss',
  standalone: true,
})
export class StocksListComponent implements OnInit {
  stocks$: Observable<StockViewModel[]> = this.store.select(StocksState.getStocks);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new LoadStocks());
  }
}
