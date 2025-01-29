import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ParcelTemplatesState } from '../../states/parcel-templates.state';
import { ApplyFilter } from '../../states/parcel-templates.action';

@Component({
    selector: 'app-parcel-templates-filters',
    templateUrl: './parcel-templates-filters.component.html',
    styleUrl: './parcel-templates-filters.component.scss',
    standalone: false
})
export class ParcelTemplatesFiltersComponent {
  searchText$: Observable<string> = this.store.select(ParcelTemplatesState.getSearchText);

  constructor(private store: Store) {}

  searchTextChanged(searchText: string) {
    this.store.dispatch(new ApplyFilter(searchText));
  }
}
