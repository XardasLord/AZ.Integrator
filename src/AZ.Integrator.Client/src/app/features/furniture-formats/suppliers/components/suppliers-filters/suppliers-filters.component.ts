import { Component, DestroyRef, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Store } from '@ngxs/store';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { ApplyFilter } from '../../states/suppliers.action';

@Component({
  selector: 'app-suppliers-filters',
  templateUrl: './suppliers-filters.component.html',
  styleUrls: ['./suppliers-filters.component.scss'],
  imports: [MaterialModule, ReactiveFormsModule],
  standalone: true,
})
export class SuppliersFiltersComponent {
  private store = inject(Store);
  private destroyRef = inject(DestroyRef);

  searchControl = new FormControl<string>('');

  constructor() {
    this.searchControl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged(), takeUntilDestroyed(this.destroyRef))
      .subscribe(value => {
        this.store.dispatch(new ApplyFilter(value || ''));
      });
  }
}
