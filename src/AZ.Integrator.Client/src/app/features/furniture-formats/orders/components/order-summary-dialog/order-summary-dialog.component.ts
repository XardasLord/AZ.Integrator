import { Component, Inject } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { EdgeBandingTypeViewModel, SupplierViewModel } from '../../../../../shared/graphql/graphql-integrator.schema';

export interface FurnitureLineData {
  furnitureCode: string;
  furnitureVersion: number;
  quantityOrdered: number;
  partLines: PartLineData[];
}

export interface PartLineData {
  partName: string;
  lengthMm: number;
  widthMm: number;
  thicknessMm: number;
  quantity: number;
  additionalInfo?: string;
  lengthEdgeBandingType: EdgeBandingTypeViewModel;
  widthEdgeBandingType: EdgeBandingTypeViewModel;
}

export interface OrderData {
  supplierId: number;
  furnitureLines: FurnitureLineData[];
  additionalNotes?: string;
}

export interface OrderSummaryData {
  orderData: OrderData;
  supplier: SupplierViewModel | undefined;
  edgeBandingTypeDisplay: (type: EdgeBandingTypeViewModel) => string;
}

@Component({
  selector: 'app-order-summary-dialog',
  templateUrl: './order-summary-dialog.component.html',
  styleUrls: ['./order-summary-dialog.component.scss'],
  standalone: true,
  imports: [MaterialModule],
})
export class OrderSummaryDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<OrderSummaryDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: OrderSummaryData
  ) {}

  onConfirm(): void {
    this.dialogRef.close(true);
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  getTotalFurnitureQuantity(): number {
    return (
      this.data.orderData.furnitureLines.reduce(
        (sum: number, line: FurnitureLineData) => sum + (line.quantityOrdered || 0),
        0
      ) || 0
    );
  }

  getTotalPartLines(): number {
    return (
      this.data.orderData.furnitureLines.reduce(
        (sum: number, line: FurnitureLineData) => sum + (line.partLines?.length || 0),
        0
      ) || 0
    );
  }

  getFurnitureDisplay(line: FurnitureLineData): string {
    return `${line.furnitureCode} (v${line.furnitureVersion})`;
  }

  getEdgeBandingDisplay(type: EdgeBandingTypeViewModel): string {
    return this.data.edgeBandingTypeDisplay(type);
  }
}
