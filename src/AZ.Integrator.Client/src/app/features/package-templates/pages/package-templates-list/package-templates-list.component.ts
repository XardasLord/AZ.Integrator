import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { LoadProductTags } from '../../states/package-templates.action';
import { PackageTemplatesState } from '../../states/package-templates.state';

@Component({
  selector: 'app-package-templates-list',
  templateUrl: './package-templates-list.component.html',
  styleUrls: ['./package-templates-list.component.scss'],
})
export class PackageTemplatesListComponent implements OnInit {
  displayedColumns: string[] = ['tags', 'actions'];
  productTags$ = this.store.select(PackageTemplatesState.getProductTags);
  totalItems$ = this.store.select(PackageTemplatesState.getProductTagsCount);
  currentPage$ = this.store.select(PackageTemplatesState.getCurrentPage);
  pageSize$ = this.store.select(PackageTemplatesState.getPageSize);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new LoadProductTags());
  }

  pageChanged(event: PageEvent): void {}

  editPackageTemplate(tag: string) {}
}
