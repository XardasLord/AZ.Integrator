import { Component, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ParcelTemplatesState } from '../../states/parcel-templates.state';
import { ApplyFilter } from '../../states/parcel-templates.action';
import { DebounceDirective } from '../../../../shared/directives/debounce.directive';
import { AsyncPipe } from '@angular/common';
import { MaterialModule } from '../../../../shared/modules/material.module';

@Component({
  selector: 'app-parcel-templates-filters',
  templateUrl: './parcel-templates-filters.component.html',
  styleUrl: './parcel-templates-filters.component.scss',
  imports: [MaterialModule, DebounceDirective, AsyncPipe],
})
export class ParcelTemplatesFiltersComponent {
  private store = inject(Store);

  searchText$: Observable<string> = this.store.select(ParcelTemplatesState.getSearchText);

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilter(searchText));
  }
}
