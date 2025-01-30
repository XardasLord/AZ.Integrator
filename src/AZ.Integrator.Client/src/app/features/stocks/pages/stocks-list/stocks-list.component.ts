import { Component, OnInit, inject } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { LoadStocks } from '../../states/stocks.action';
import { StockViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { StocksState } from '../../states/stocks.state';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-stocks-list',
  imports: [SharedModule],
  templateUrl: './stocks-list.component.html',
  styleUrl: './stocks-list.component.scss',
  standalone: true,
})
export class StocksListComponent implements OnInit {
  private store = inject(Store);

  stockWarningThreshold = environment.stockWarningThreshold;
  stocks$: Observable<StockViewModel[]> = this.store.select(StocksState.getStocks);

  ngOnInit(): void {
    this.store.dispatch(new LoadStocks());
  }
}
