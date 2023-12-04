import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { LoadProductTags } from '../../states/package-templates.action';
import { PackageTemplatesState } from '../../states/package-templates.state';

@Component({
  selector: 'app-package-templates-list',
  templateUrl: './package-templates-list.component.html',
  styleUrls: ['./package-templates-list.component.scss'],
})
export class PackageTemplatesListComponent implements OnInit {
  productTags$ = this.store.select(PackageTemplatesState.getProductTags);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new LoadProductTags());
  }
}
