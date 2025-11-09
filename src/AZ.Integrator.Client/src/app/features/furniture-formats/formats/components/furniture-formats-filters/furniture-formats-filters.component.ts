import { Component, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Store } from '@ngxs/store';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { ApplyFilter } from '../../states/formats.action';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-furniture-formats-filters',
  templateUrl: './furniture-formats-filters.component.html',
  styleUrls: ['./furniture-formats-filters.component.scss'],
  imports: [MaterialModule, ReactiveFormsModule, NgIf],
  standalone: true,
})
export class FurnitureFormatsFiltersComponent {
  private store = inject(Store);

  searchControl = new FormControl<string>('');

  constructor() {
    this.searchControl.valueChanges.pipe(debounceTime(300), distinctUntilChanged()).subscribe(value => {
      this.store.dispatch(new ApplyFilter(value || ''));
    });
  }
}
