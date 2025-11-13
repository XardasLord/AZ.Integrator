import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { FurnitureSelectorComponent } from '../furniture-selector/furniture-selector.component';
import { OrderFormGroup } from '../../models/order-form-group.model';
import { FurnitureLineFormGroup } from '../../models/furniture-line-form-group.model';
import { PartLineFormGroup } from '../../models/part-line-form-group.model';
import {
  EdgeBandingTypeViewModel,
  FurnitureModelViewModel,
  PartDefinitionViewModel,
  SupplierViewModel,
} from '../../../../../shared/graphql/graphql-integrator.schema';
import { SuppliersState } from '../../../suppliers/states/suppliers.state';
import { FormatsState } from '../../../formats/states/formats.state';
import { LoadSuppliers } from '../../../suppliers/states/suppliers.action';
import { LoadAllFurnitureDefinitions } from '../../../formats/states/formats.action';
import { CreateOrder } from '../../states/orders.action';
import {
  FurnitureLineData,
  OrderData,
  OrderSummaryDialogComponent,
  PartLineData,
} from '../order-summary-dialog/order-summary-dialog.component';
import { EdgeBandingTypeHelper } from '../../../formats/helpers/edge-banding-type.helper';
import { Navigate } from '@ngxs/router-plugin';
import { FurnitureFormatsRoutePath } from '../../../../../core/modules/app-routing.module';

@Component({
  selector: 'app-create-order-form',
  templateUrl: './create-order-form.component.html',
  styleUrls: ['./create-order-form.component.scss'],
  imports: [MaterialModule, CommonModule, ReactiveFormsModule, FurnitureSelectorComponent],
  standalone: true,
})
export class CreateOrderFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private store = inject(Store);
  private dialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

  orderForm!: FormGroup<OrderFormGroup>;
  suppliers$: Observable<SupplierViewModel[]> = this.store.select(SuppliersState.getSuppliers);
  furnitureDefinitions$: Observable<FurnitureModelViewModel[]> = this.store.select(
    FormatsState.getFurnitureDefinitions
  );

  selectedFurnitureDefinitions: FurnitureModelViewModel[] = [];
  edgeBandingTypes = Object.values(EdgeBandingTypeViewModel);

  ngOnInit(): void {
    this.initForm();
    this.loadData();
  }

  private initForm(): void {
    this.orderForm = this.fb.group<OrderFormGroup>({
      supplierId: this.fb.control(null, [Validators.required]),
      furnitureLines: this.fb.array<FormGroup<FurnitureLineFormGroup>>([]),
      additionalNotes: this.fb.control(''),
    });
  }

  private loadData(): void {
    this.store.dispatch(new LoadSuppliers());
    this.store.dispatch(new LoadAllFurnitureDefinitions());
  }

  get furnitureLines(): FormArray<FormGroup<FurnitureLineFormGroup>> {
    return this.orderForm.get('furnitureLines') as FormArray<FormGroup<FurnitureLineFormGroup>>;
  }

  onFurnitureSelectionChange(selectedDefinitions: FurnitureModelViewModel[]): void {
    this.selectedFurnitureDefinitions = selectedDefinitions;
    this.updateFurnitureLines();
  }

  private updateFurnitureLines(): void {
    const currentLines = this.furnitureLines;

    // Remove lines that are no longer selected
    for (let i = currentLines.length - 1; i >= 0; i--) {
      const line = currentLines.at(i);
      const furnitureCode = line.get('furnitureCode')?.value;
      const furnitureVersion = line.get('furnitureVersion')?.value;

      const stillSelected = this.selectedFurnitureDefinitions.some(
        def => def.furnitureCode === furnitureCode && def.version === furnitureVersion
      );

      if (!stillSelected) {
        currentLines.removeAt(i);
      }
    }

    // Order part definitions by id
    this.selectedFurnitureDefinitions = this.selectedFurnitureDefinitions.map(definition => ({
      ...definition,
      partDefinitions: [...definition.partDefinitions].sort((a, b) => a.id - b.id),
    }));

    // Add new lines for newly selected definitions
    this.selectedFurnitureDefinitions.forEach(definition => {
      const exists = currentLines.controls.some(
        line =>
          line.get('furnitureCode')?.value === definition.furnitureCode &&
          line.get('furnitureVersion')?.value === definition.version
      );

      if (!exists) {
        currentLines.push(this.createFurnitureLineFormGroup(definition));
      }
    });
  }

  private createFurnitureLineFormGroup(definition: FurnitureModelViewModel): FormGroup<FurnitureLineFormGroup> {
    const partLines = this.fb.array<FormGroup<PartLineFormGroup>>(
      definition.partDefinitions.map(part => this.createPartLineFormGroup(part))
    );

    return this.fb.group<FurnitureLineFormGroup>({
      furnitureCode: this.fb.control(definition.furnitureCode, [Validators.required]),
      furnitureVersion: this.fb.control(definition.version, [Validators.required]),
      quantityOrdered: this.fb.control(1, [Validators.required, Validators.min(1)]),
      partLines: partLines,
    });
  }

  private createPartLineFormGroup(part: PartDefinitionViewModel): FormGroup<PartLineFormGroup> {
    return this.fb.group<PartLineFormGroup>({
      partName: this.fb.control(part.name, [Validators.required]),
      lengthMm: this.fb.control(part.dimensions.lengthMm, [Validators.required, Validators.min(1)]),
      widthMm: this.fb.control(part.dimensions.widthMm, [Validators.required, Validators.min(1)]),
      thicknessMm: this.fb.control(part.dimensions.thicknessMm, [Validators.required, Validators.min(1)]),
      quantity: this.fb.control(part.quantity, [Validators.required, Validators.min(1)]),
      additionalInfo: this.fb.control(part.additionalInfo || ''),
      lengthEdgeBandingType: this.fb.control(part.dimensions.lengthEdgeBandingType || EdgeBandingTypeViewModel.None, [
        Validators.required,
      ]),
      widthEdgeBandingType: this.fb.control(part.dimensions.widthEdgeBandingType || EdgeBandingTypeViewModel.None, [
        Validators.required,
      ]),
    });
  }

  getPartLines(furnitureLineIndex: number): FormArray<FormGroup<PartLineFormGroup>> {
    return this.furnitureLines.at(furnitureLineIndex).get('partLines') as FormArray<FormGroup<PartLineFormGroup>>;
  }

  addPartLine(furnitureLineIndex: number): void {
    const partLines = this.getPartLines(furnitureLineIndex);
    const newPartIndex = partLines.length;

    partLines.push(
      this.fb.group<PartLineFormGroup>({
        partName: this.fb.control('', [Validators.required]),
        lengthMm: this.fb.control(null, [Validators.required, Validators.min(1)]),
        widthMm: this.fb.control(null, [Validators.required, Validators.min(1)]),
        thicknessMm: this.fb.control(null, [Validators.required, Validators.min(1)]),
        quantity: this.fb.control(1, [Validators.required, Validators.min(1)]),
        additionalInfo: this.fb.control(''),
        lengthEdgeBandingType: this.fb.control(EdgeBandingTypeViewModel.None, [Validators.required]),
        widthEdgeBandingType: this.fb.control(EdgeBandingTypeViewModel.None, [Validators.required]),
      })
    );

    // Scroll to the newly added part line after the view updates
    setTimeout(() => {
      const elementId = `part-line-${furnitureLineIndex}-${newPartIndex}`;
      const element = document.getElementById(elementId);
      if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'center' });
      }
    }, 150);
  }

  removePartLine(furnitureLineIndex: number, partLineIndex: number): void {
    const partLines = this.getPartLines(furnitureLineIndex);
    partLines.removeAt(partLineIndex);
  }

  getFurnitureDefinitionDisplay(furnitureLine: FormGroup<FurnitureLineFormGroup>): string {
    const code = furnitureLine.get('furnitureCode')?.value;
    const version = furnitureLine.get('furnitureVersion')?.value;
    return `${code} (v${version})`;
  }

  onSubmit(): void {
    if (this.orderForm.valid) {
      const formValue = this.prepareFormData();
      const suppliers = this.store.selectSnapshot(SuppliersState.getSuppliers);
      const supplier = suppliers.find(s => s.id === formValue.supplierId);

      const dialogRef = this.dialog.open(OrderSummaryDialogComponent, {
        width: '800px',
        maxHeight: '90vh',
        data: {
          orderData: formValue,
          supplier: supplier,
          edgeBandingTypeDisplay: this.getEdgeBandingTypeDisplay.bind(this),
        },
      });

      dialogRef
        .afterClosed()
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe(confirmed => {
          if (confirmed) {
            this.submitOrder(formValue);
          }
        });
    }
  }

  private prepareFormData(): OrderData {
    const supplierId = this.orderForm.get('supplierId')!.value;
    const additionalNotes = this.orderForm.get('additionalNotes')?.value;
    const furnitureLines: FurnitureLineData[] = [];

    this.furnitureLines.controls.forEach((furnitureLineControl, furnitureIndex) => {
      const partLines: PartLineData[] = [];
      const partLinesArray = furnitureLineControl.get('partLines') as FormArray<FormGroup<PartLineFormGroup>>;

      partLinesArray.controls.forEach((partLineControl, partIndex) => {
        const partName = partLineControl.get('partName')?.value!;

        partLines.push({
          partName: partName,
          lengthMm: partLineControl.get('lengthMm')?.value!,
          widthMm: partLineControl.get('widthMm')?.value!,
          thicknessMm: partLineControl.get('thicknessMm')?.value!,
          quantity: partLineControl.get('quantity')?.value!,
          additionalInfo: partLineControl.get('additionalInfo')?.value || '',
          lengthEdgeBandingType: partLineControl.get('lengthEdgeBandingType')?.value!,
          widthEdgeBandingType: partLineControl.get('widthEdgeBandingType')?.value!,
        });
      });

      furnitureLines.push({
        furnitureCode: furnitureLineControl.get('furnitureCode')?.value!,
        furnitureVersion: furnitureLineControl.get('furnitureVersion')?.value!,
        quantityOrdered: furnitureLineControl.get('quantityOrdered')?.value!,
        partLines: partLines,
      });
    });

    return {
      supplierId: supplierId!,
      furnitureLines: furnitureLines,
      additionalNotes: additionalNotes || undefined,
    };
  }

  private submitOrder(formValue: OrderData): void {
    const furnitureLineRequests = formValue.furnitureLines.map((line: FurnitureLineData) => ({
      furnitureCode: line.furnitureCode!,
      furnitureVersion: line.furnitureVersion!,
      quantityOrdered: line.quantityOrdered!,
      partDefinitionLines:
        line.partLines?.map((part: PartLineData) => ({
          partName: part.partName!,
          lengthMm: part.lengthMm!,
          widthMm: part.widthMm!,
          thicknessMm: part.thicknessMm!,
          quantity: part.quantity!,
          additionalInfo: part.additionalInfo || '',
          lengthEdgeBandingType: EdgeBandingTypeHelper.toNumber(part.lengthEdgeBandingType!),
          widthEdgeBandingType: EdgeBandingTypeHelper.toNumber(part.widthEdgeBandingType!),
        })) || [],
    }));

    this.store.dispatch(new CreateOrder(formValue.supplierId!, furnitureLineRequests, formValue.additionalNotes));
  }

  onCancel(): void {
    this.store.dispatch(new Navigate([FurnitureFormatsRoutePath.Orders]));
  }

  getEdgeBandingTypeDisplay(type: EdgeBandingTypeViewModel): string {
    const displayMap: Record<EdgeBandingTypeViewModel, string> = {
      [EdgeBandingTypeViewModel.None]: 'Brak',
      [EdgeBandingTypeViewModel.One]: 'Jednostronna',
      [EdgeBandingTypeViewModel.Two]: 'Dwustronna',
    };
    return displayMap[type] || type;
  }
}
