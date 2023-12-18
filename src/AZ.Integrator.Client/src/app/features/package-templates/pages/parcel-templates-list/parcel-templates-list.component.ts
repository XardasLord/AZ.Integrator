import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { ParcelTemplatesState } from '../../states/parcel-templates.state';
import { LoadProductTags, OpenPackageTemplateDefinitionModal } from '../../states/parcel-templates.action';

@Component({
  selector: 'app-parcel-templates-list',
  templateUrl: './parcel-templates-list.component.html',
  styleUrls: ['./parcel-templates-list.component.scss'],
})
export class ParcelTemplatesListComponent implements OnInit {
  displayedColumns: string[] = ['tags', 'actions'];
  productTags$ = this.store.select(ParcelTemplatesState.getProductTags);
  totalItems$ = this.store.select(ParcelTemplatesState.getProductTagsCount);
  currentPage$ = this.store.select(ParcelTemplatesState.getCurrentPage);
  pageSize$ = this.store.select(ParcelTemplatesState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new LoadProductTags());
  }

  pageChanged(event: PageEvent): void {}

  editPackageTemplate(tag: string) {
    this.store.dispatch(new OpenPackageTemplateDefinitionModal(tag));
  }
}
