import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { UserScanningStats } from '../stocks-statistics/stocks-statistics.component';

import * as echarts from 'echarts/core';
import { EChartsCoreOption } from 'echarts/core';
import { BarChart, LineChart } from 'echarts/charts';
import { GridComponent, TooltipComponent } from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';
import { NgxEchartsDirective, provideEchartsCore } from 'ngx-echarts';

echarts.use([LineChart, BarChart, GridComponent, TooltipComponent, CanvasRenderer]);

@Component({
  selector: 'app-stocks-statistics-charts',
  imports: [SharedModule, NgxEchartsDirective],
  templateUrl: './stocks-statistics-charts.component.html',
  styleUrl: './stocks-statistics-charts.component.scss',
  standalone: true,
  providers: [provideEchartsCore({ echarts })],
})
export class StocksStatisticsChartsComponent {
  @Input() groupedLogs$!: Observable<UserScanningStats[]>;

  getBarChartOption(data: UserScanningStats[]): EChartsCoreOption {
    return {
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'shadow',
        },
      },
      xAxis: {
        type: 'category',
        data: data.map(d => d.createdBy),
        axisLabel: {
          rotate: 25,
        },
      },
      yAxis: {
        type: 'value',
        name: 'Ilość skanowań',
      },
      series: [
        {
          name: 'Ilość skanowań',
          type: 'bar',
          data: data.map((d, index) => ({
            value: d.totalScanned,
            itemStyle: { color: getColor(index) },
          })),
          emphasis: {
            focus: 'series',
          },
        },
      ],
      grid: {
        left: '10%',
        right: '10%',
        bottom: '15%',
        containLabel: true,
      },
    };
  }
}

const colors = ['#5470c6', '#91cc75', '#fac858', '#ee6666', '#73c0de', '#3ba272', '#fc8452', '#9a60b4', '#ea7ccc'];
function getColor(index: number): string {
  return colors[index % colors.length]; // Zapętlamy kolory
}
