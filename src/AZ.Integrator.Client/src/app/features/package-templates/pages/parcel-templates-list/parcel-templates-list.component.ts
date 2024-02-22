import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { PageEvent } from '@angular/material/paginator';
import { ParcelTemplatesState } from '../../states/parcel-templates.state';
import { ChangePage, LoadProductTags, OpenPackageTemplateDefinitionModal } from '../../states/parcel-templates.action';

@Component({
  selector: 'app-parcel-templates-list',
  templateUrl: './parcel-templates-list.component.html',
  styleUrls: ['./parcel-templates-list.component.scss'],
})
export class ParcelTemplatesListComponent implements OnInit {
  displayedColumns: string[] = ['signatures', 'actions'];
  productTags$ = this.store.select(ParcelTemplatesState.getProductTags);
  totalItems$ = this.store.select(ParcelTemplatesState.getProductTagsCount);
  currentPage$ = this.store.select(ParcelTemplatesState.getCurrentPage);
  pageSize$ = this.store.select(ParcelTemplatesState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new LoadProductTags());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  editPackageTemplate(tag: string) {
    this.store.dispatch(new OpenPackageTemplateDefinitionModal(tag));
  }
}
