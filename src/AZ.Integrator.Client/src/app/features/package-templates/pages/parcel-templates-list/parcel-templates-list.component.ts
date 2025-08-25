import { Component, inject, OnInit } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { ParcelTemplatesState } from '../../states/parcel-templates.state';
import { ChangePage, LoadTemplates, SavePackageTemplate } from '../../states/parcel-templates.action';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { ScrollTableComponent } from '../../../../shared/ui/wrappers/scroll-table/scroll-table.component';
import { TagParcelTemplateViewModel } from '../../../../shared/graphql/graphql-integrator.schema';
import { PackageTemplateDefinitionFormDialogComponent } from '../../components/package-template-definition-form-dialog/package-template-definition-form-dialog.component';
import { PackageTemplateDefinitionFormDialogResponseModel } from '../../components/package-template-definition-form-dialog/package-template-definition-form-dialog-response-model';
import { MatDialog } from '@angular/material/dialog';
import { ParcelTemplateDefinitionDataModel } from '../../components/package-template-definition-form-dialog/parcel-template-definition-data.model';

@Component({
  selector: 'app-parcel-templates-list',
  templateUrl: './parcel-templates-list.component.html',
  styleUrls: ['./parcel-templates-list.component.scss'],
  imports: [MaterialModule, AsyncPipe, ScrollTableComponent],
})
export class ParcelTemplatesListComponent implements OnInit {
  private store = inject(Store);
  private dialog = inject(MatDialog);

  displayedColumns: string[] = ['tags', 'actions'];
  templates$ = this.store.select(ParcelTemplatesState.getTemplates);
  totalItems$ = this.store.select(ParcelTemplatesState.getTemplatesCount);
  currentPage$ = this.store.select(ParcelTemplatesState.getCurrentPage);
  pageSize$ = this.store.select(ParcelTemplatesState.getPageSize);

  ngOnInit(): void {
    this.store.dispatch(new LoadTemplates());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  editPackageTemplate(template: TagParcelTemplateViewModel) {
    const dialogRef = this.dialog.open(PackageTemplateDefinitionFormDialogComponent, {
      width: '50%',
      height: '70%',
      data: <ParcelTemplateDefinitionDataModel>{
        tag: template.tag,
        template: template,
      },
    });

    dialogRef.afterClosed().subscribe((result: PackageTemplateDefinitionFormDialogResponseModel) => {
      if (!result) {
        return;
      }

      this.store.dispatch(
        new SavePackageTemplate({
          tag: result.tag,
          parcelTemplates: result.parcels,
        })
      );
    });
  }
}
