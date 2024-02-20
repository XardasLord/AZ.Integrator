import { Component, OnInit } from '@angular/core';
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

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new LoadProductTags());
  }

  editPackageTemplate(tag: string) {
    this.store.dispatch(new OpenPackageTemplateDefinitionModal(tag));
  }
}
