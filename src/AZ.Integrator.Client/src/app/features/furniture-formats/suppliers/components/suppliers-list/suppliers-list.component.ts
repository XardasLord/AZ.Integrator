import { Component, inject, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { AsyncPipe, CommonModule, DatePipe } from '@angular/common';
import { Store } from '@ngxs/store';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { SuppliersState } from '../../states/suppliers.state';
import { ChangePage, DeleteSupplier, LoadSuppliers } from '../../states/suppliers.action';
import { SupplierFormDialogComponent } from '../supplier-form-dialog/supplier-form-dialog.component';
import { ScrollTableComponent } from '../../../../../shared/ui/wrappers/scroll-table/scroll-table.component';
import { SupplierViewModel } from '../../../../../shared/graphql/graphql-integrator.schema';

@Component({
  selector: 'app-suppliers-list',
  templateUrl: './suppliers-list.component.html',
  styleUrls: ['./suppliers-list.component.scss'],
  imports: [MaterialModule, AsyncPipe, ScrollTableComponent, DatePipe, CommonModule],
  standalone: true,
})
export class SuppliersListComponent implements OnInit {
  private store = inject(Store);
  private dialog = inject(MatDialog);

  displayedColumns: string[] = ['name', 'telephoneNumber', 'mailboxesCount', 'actions'];
  suppliers$ = this.store.select(SuppliersState.getSuppliers);
  totalItems$ = this.store.select(SuppliersState.getTotalCount);
  currentPage$ = this.store.select(SuppliersState.getCurrentPage);
  pageSize$ = this.store.select(SuppliersState.getPageSize);

  ngOnInit(): void {
    this.store.dispatch(new LoadSuppliers());
  }

  pageChanged(event: PageEvent): void {
    this.store.dispatch(new ChangePage(event));
  }

  editSupplier(supplier: SupplierViewModel): void {
    const dialogRef = this.dialog.open(SupplierFormDialogComponent, {
      width: '50%',
      maxHeight: '90vh',
      data: supplier,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Refresh handled by state
      }
    });
  }

  deleteSupplier(supplierId: number): void {
    this.store.dispatch(new DeleteSupplier(supplierId));
  }

  getMailboxesDisplay(supplier: SupplierViewModel): string {
    if (!supplier.mailboxes || supplier.mailboxes.length === 0) {
      return '-';
    }
    return supplier.mailboxes.map((m: { email: string }) => m.email).join(', ');
  }
}
