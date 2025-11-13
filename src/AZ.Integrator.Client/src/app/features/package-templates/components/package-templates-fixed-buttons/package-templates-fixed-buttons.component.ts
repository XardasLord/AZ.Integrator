import { Component, DestroyRef, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { Store } from '@ngxs/store';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { SharedModule } from '../../../../shared/shared.module';
import { PackageTemplateDefinitionFormDialogComponent } from '../package-template-definition-form-dialog/package-template-definition-form-dialog.component';
import { LoadTemplates, SavePackageTemplate } from '../../states/parcel-templates.action';
import { PackageTemplateDefinitionFormDialogResponseModel } from '../package-template-definition-form-dialog/package-template-definition-form-dialog-response-model';

@Component({
  selector: 'app-package-templates-fixed-buttons',
  imports: [SharedModule],
  templateUrl: './package-templates-fixed-buttons.component.html',
  styleUrl: './package-templates-fixed-buttons.component.scss',
  standalone: true,
})
export class PackageTemplatesFixedButtonsComponent {
  private store = inject(Store);
  private dialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

  addTemplate() {
    const dialogRef = this.dialog.open(PackageTemplateDefinitionFormDialogComponent, {
      width: '50%',
      height: '70%',
      data: null,
    });

    dialogRef
      .afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((result: PackageTemplateDefinitionFormDialogResponseModel) => {
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

  refreshTemplates() {
    this.store.dispatch(new LoadTemplates());
  }
}
