import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Injectable, inject } from '@angular/core';
import { ProgressSpinnerComponent } from '../components/progress-spinner/progress-spinner.component';

@Injectable({
  providedIn: 'root',
})
export class ProgressSpinnerService {
  private dialog = inject(MatDialog);

  private progressSpinnerDialogRef?: MatDialogRef<ProgressSpinnerComponent>;

  hideProgressSpinner(): void {
    if (this.progressSpinnerDialogRef) {
      this.progressSpinnerDialogRef.close();

      this.progressSpinnerDialogRef = undefined;
    }
  }

  showProgressSpinner(): void {
    this.hideProgressSpinner();
    this.progressSpinnerDialogRef = this.dialog.open(ProgressSpinnerComponent, {
      panelClass: 'transparent',
      disableClose: true,
    });
  }
}
