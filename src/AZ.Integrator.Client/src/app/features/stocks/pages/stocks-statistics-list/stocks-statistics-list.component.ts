import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { UserScanningStats } from '../stocks-statistics/stocks-statistics.component';

@Component({
  selector: 'app-stocks-statistics-list',
  imports: [SharedModule],
  templateUrl: './stocks-statistics-list.component.html',
  styleUrl: './stocks-statistics-list.component.scss',
  standalone: true,
})
export class StocksStatisticsListComponent {
  @Input() groupedLogs$!: Observable<UserScanningStats[]>;
}
