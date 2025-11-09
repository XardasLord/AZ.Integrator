import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NgForOf, NgIf } from '@angular/common';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Store } from '@ngxs/store';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { FurnitureDefinitionFormGroup } from '../../models/furniture-definition-form-group.model';
import { PartDefinitionFormGroup } from '../../models/part-definition-form-group.model';
import { SaveFurnitureDefinitionCommand } from '../../models/commands/save-furniture-definition.command';
import { PartDefinitionDto } from '../../models/part-definition.dto';
import { AddFurnitureDefinition, UpdateFurnitureDefinition } from '../../states/formats.action';
import {
  EdgeBandingTypeViewModel,
  FurnitureModelViewModel,
  PartDefinitionViewModel,
} from '../../../../../shared/graphql/graphql-integrator.schema';
import { EdgeBandingTypeHelper } from '../../helpers/edge-banding-type.helper';
import { ExcelImportService } from '../../services/excel-import.service';
import { ToastrService } from 'ngx-toastr';

interface PartFormValue {
  id?: number | null;
  name?: string | null;
  lengthMm?: number | null;
  widthMm?: number | null;
  thicknessMm?: number | null;
  lengthEdgeBandingType?: EdgeBandingTypeViewModel | null;
  widthEdgeBandingType?: EdgeBandingTypeViewModel | null;
  quantity?: number | null;
  additionalInfo?: string | null;
}

@Component({
  selector: 'app-furniture-definition-form-dialog',
  templateUrl: './furniture-definition-form-dialog.component.html',
  styleUrls: ['./furniture-definition-form-dialog.component.scss'],
  imports: [MaterialModule, FormsModule, ReactiveFormsModule, NgIf, NgForOf],
  standalone: true,
})
export class FurnitureDefinitionFormDialogComponent implements OnInit {
  private data: FurnitureModelViewModel | null = inject(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<FurnitureDefinitionFormDialogComponent>);
  private store = inject(Store);
  private excelImportService = inject(ExcelImportService);
  private toastr = inject(ToastrService);

  form!: FormGroup<FurnitureDefinitionFormGroup>;
  editMode: boolean = false;
  isImporting: boolean = false;
  isDragOver: boolean = false;

  edgeBandingOptions = [
    { value: EdgeBandingTypeViewModel.None, label: 'Brak' },
    { value: EdgeBandingTypeViewModel.One, label: 'Jedna krawędź' },
    { value: EdgeBandingTypeViewModel.Two, label: 'Dwie krawędzie' },
  ];

  get partDefinitions(): FormArray<FormGroup<PartDefinitionFormGroup>> {
    return this.form.controls.partDefinitions;
  }

  ngOnInit(): void {
    this.editMode = !!this.data?.furnitureCode;

    this.form = this.fb.group<FurnitureDefinitionFormGroup>({
      furnitureCode: new FormControl<string | null>(
        {
          value: this.editMode ? this.data!.furnitureCode : '',
          disabled: this.editMode,
        },
        [Validators.required, Validators.minLength(1)]
      ),
      partDefinitions: this.fb.array<FormGroup<PartDefinitionFormGroup>>(
        [],
        [Validators.required, Validators.minLength(1)]
      ),
    });

    if (this.data?.partDefinitions && this.data.partDefinitions.length > 0) {
      this.data.partDefinitions.forEach((part: PartDefinitionViewModel) => {
        this.addPartDefinition(
          part.name,
          part.dimensions.lengthMm,
          part.dimensions.widthMm,
          part.dimensions.thicknessMm,
          part.dimensions.lengthEdgeBandingType,
          part.dimensions.widthEdgeBandingType,
          part.quantity,
          part.additionalInfo!
        );
      });
    } else {
      this.addPartDefinition();
    }

    this.form.markAllAsTouched();
  }

  addPartDefinition(
    name = '',
    lengthMm = 0,
    widthMm = 0,
    thicknessMm = 0,
    lengthEdgeBandingType = EdgeBandingTypeViewModel.None,
    widthEdgeBankingType = EdgeBandingTypeViewModel.None,
    quantity = 1,
    additionalInfo = ''
  ): void {
    const partGroup = this.fb.group<PartDefinitionFormGroup>({
      id: new FormControl(null),
      name: new FormControl<string>(name, [Validators.required]),
      lengthMm: new FormControl<number>(lengthMm, [Validators.required, Validators.min(1)]),
      widthMm: new FormControl<number>(widthMm, [Validators.required, Validators.min(1)]),
      thicknessMm: new FormControl<number>(thicknessMm, [Validators.required, Validators.min(1)]),
      lengthEdgeBandingType: new FormControl<EdgeBandingTypeViewModel>(lengthEdgeBandingType, [Validators.required]),
      widthEdgeBandingType: new FormControl<EdgeBandingTypeViewModel>(widthEdgeBankingType, [Validators.required]),
      quantity: new FormControl<number>(quantity, [Validators.required, Validators.min(1)]),
      additionalInfo: new FormControl<string>(additionalInfo, []),
    });

    this.partDefinitions.push(partGroup);
  }

  removePartDefinition(index: number): void {
    this.partDefinitions.removeAt(index);
  }

  onSubmit(): void {
    if (!this.form.valid) {
      return;
    }

    const partDefinitions: PartDefinitionDto[] = this.form.value.partDefinitions!.map((part: PartFormValue) => ({
      id: part.id || null,
      name: part.name!,
      lengthMm: part.lengthMm!,
      widthMm: part.widthMm!,
      thicknessMm: part.thicknessMm!,
      lengthEdgeBandingType: EdgeBandingTypeHelper.toNumber(part.lengthEdgeBandingType!),
      widthEdgeBandingType: EdgeBandingTypeHelper.toNumber(part.widthEdgeBandingType!),
      quantity: part.quantity!,
      additionalInfo: part.additionalInfo || '',
    }));

    const command: SaveFurnitureDefinitionCommand = {
      furnitureCode: this.editMode ? this.data!.furnitureCode : this.form.value.furnitureCode!,
      partDefinitions,
    };

    const action = this.editMode ? new UpdateFurnitureDefinition(command) : new AddFurnitureDefinition(command);

    this.store.dispatch(action).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: () => {
        // Error handled by state
      },
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  /**
   * Obsługuje kliknięcie przycisku importu z Excel
   */
  onImportFromExcel(): void {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = '.xlsx,.xls';
    input.onchange = (event: Event) => {
      const target = event.target as HTMLInputElement;
      const file = target.files?.[0];
      if (file) {
        this.importExcelFile(file);
      }
    };
    input.click();
  }

  /**
   * Importuje dane z pliku Excel i wypełnia formularz
   */
  private async importExcelFile(file: File): Promise<void> {
    this.isImporting = true;

    try {
      const importData = await this.excelImportService.importFurnitureFromExcel(file);

      // Wypełnij kod mebla (tylko jeśli nie jesteśmy w trybie edycji)
      if (!this.editMode) {
        this.form.controls.furnitureCode.setValue(importData.furnitureCode);
      }

      // Wyczyść istniejące formatki
      this.partDefinitions.clear();

      // Dodaj formatki z importu
      importData.parts.forEach(part => {
        this.addPartDefinition(
          part.name,
          part.lengthMm,
          part.widthMm,
          part.thicknessMm,
          EdgeBandingTypeHelper.fromNumber(part.lengthEdgeBandingType),
          EdgeBandingTypeHelper.fromNumber(part.widthEdgeBandingType),
          part.quantity,
          part.additionalInfo
        );
      });

      this.toastr.success(`Zaimportowano ${importData.parts.length} formatek`, 'Import zakończony sukcesem');
    } catch (error: unknown) {
      console.error('Błąd importu:', error);
      const errorMessage = error instanceof Error ? error.message : 'Wystąpił błąd podczas importu pliku Excel';
      this.toastr.error(errorMessage, 'Błąd importu');
    } finally {
      this.isImporting = false;
    }
  }

  /**
   * Obsługuje zdarzenie upuszczenia pliku
   */
  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = false;

    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      const file = files[0];
      // Sprawdź czy to plik Excel
      if (file.name.endsWith('.xlsx') || file.name.endsWith('.xls')) {
        this.importExcelFile(file);
      } else {
        this.toastr.error('Proszę wybrać plik Excel (.xlsx lub .xls)', 'Nieprawidłowy format pliku');
      }
    }
  }

  /**
   * Obsługuje zdarzenie przeciągania nad obszarem
   */
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = true;
  }

  /**
   * Obsługuje zdarzenie opuszczenia obszaru podczas przeciągania
   */
  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = false;
  }
}
