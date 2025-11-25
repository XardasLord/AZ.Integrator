import { Component, inject, Inject, Optional } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Store } from '@ngxs/store';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { IntegrationType, IntegrationTypeLabels, IntegrationTypeLogos } from '../../models/integration-type.enum';
import { IntegrationsService } from '../../services/integrations.service';
import { LoadIntegrations } from '../../states/integrations.action';
import { ToastrService } from 'ngx-toastr';
import { IntegrationWithType } from '../../models/integration.model';

@Component({
  selector: 'app-add-integration-dialog',
  standalone: true,
  templateUrl: './add-integration-dialog.component.html',
  styleUrls: ['./add-integration-dialog.component.scss'],
  imports: [CommonModule, MaterialModule, FormsModule, ReactiveFormsModule],
})
export class AddIntegrationDialogComponent {
  private dialogRef = inject(MatDialogRef<AddIntegrationDialogComponent>);
  private fb = inject(FormBuilder);
  private integrationsService = inject(IntegrationsService);
  private store = inject(Store);
  private toastr = inject(ToastrService);

  selectedType: IntegrationType | null = null;
  integrationForm: FormGroup;
  loading = false;
  isEditMode = false;

  integrationTypes = Object.values(IntegrationType);
  IntegrationType = IntegrationType;

  constructor(@Optional() @Inject(MAT_DIALOG_DATA) public data: IntegrationWithType | null) {
    this.integrationForm = this.fb.group({});

    // Tryb edycji - automatycznie wybierz typ i wypełnij formularz
    if (data) {
      this.isEditMode = true;
      this.selectedType = data.type;
      this.buildForm(data.type, data.integration);
    }
  }

  getIntegrationTypeLabel(type: IntegrationType): string {
    return IntegrationTypeLabels[type];
  }

  getIntegrationLogo(type: IntegrationType): string {
    return IntegrationTypeLogos[type];
  }

  selectType(type: IntegrationType): void {
    this.selectedType = type;
    this.buildForm(type);
  }

  buildForm(type: IntegrationType, integration?: any): void {
    const data = integration || {};

    switch (type) {
      case IntegrationType.Allegro:
        // Allegro używa OAuth, nie potrzebuje formularza
        this.integrationForm = this.fb.group({
          displayName: [data.displayName || '', Validators.required],
        });
        break;

      case IntegrationType.Erli:
        this.integrationForm = this.fb.group({
          sourceSystemId: [{ value: data.sourceSystemId || '', disabled: this.isEditMode }, Validators.required],
          apiKey: [data.apiKey || '', Validators.required],
          displayName: [data.displayName || '', Validators.required],
        });
        break;

      case IntegrationType.Shopify:
        this.integrationForm = this.fb.group({
          sourceSystemId: [{ value: data.sourceSystemId || '', disabled: this.isEditMode }, Validators.required],
          apiUrl: [data.apiUrl || '', [Validators.required, Validators.pattern(/^https?:\/\/.+/)]],
          adminApiToken: [data.adminApiToken || '', Validators.required],
          displayName: [data.displayName || '', Validators.required],
        });
        break;

      case IntegrationType.Fakturownia:
        this.integrationForm = this.fb.group({
          sourceSystemId: [{ value: data.sourceSystemId || '', disabled: this.isEditMode }, Validators.required],
          apiKey: [data.apiKey || '', Validators.required],
          apiUrl: [data.apiUrl || '', [Validators.required, Validators.pattern(/^https?:\/\/.+/)]],
          displayName: [data.displayName || '', Validators.required],
        });
        break;

      case IntegrationType.Inpost:
        this.integrationForm = this.fb.group({
          organizationId: [
            { value: data.organizationId || '', disabled: this.isEditMode },
            [Validators.required, Validators.pattern(/^\d+$/)],
          ],
          accessToken: [data.accessToken || '', Validators.required],
          displayName: [data.displayName || '', Validators.required],
          senderName: [data.senderName || '', Validators.required],
          senderCompanyName: [data.senderCompanyName || '', Validators.required],
          senderFirstName: [data.senderFirstName || '', Validators.required],
          senderLastName: [data.senderLastName || '', Validators.required],
          senderEmail: [data.senderEmail || '', [Validators.required, Validators.email]],
          senderPhone: [data.senderPhone || '', Validators.required],
          senderAddressStreet: [data.senderAddressStreet || '', Validators.required],
          senderAddressBuildingNumber: [data.senderAddressBuildingNumber || '', Validators.required],
          senderAddressCity: [data.senderAddressCity || '', Validators.required],
          senderAddressPostCode: [data.senderAddressPostCode || '', Validators.required],
          senderAddressCountryCode: [data.senderAddressCountryCode || 'PL', Validators.required],
        });
        break;

      case IntegrationType.Dpd:
        this.integrationForm = this.fb.group({
          login: [
            { value: data.login || '', disabled: this.isEditMode },
            [Validators.required, Validators.pattern(/^\d+$/)],
          ],
          password: ['', this.isEditMode ? [] : [Validators.required]], // Password nie wymagane przy edycji
          masterFid: [data.masterFid || '', [Validators.required, Validators.pattern(/^\d+$/)]],
          senderName: [data.senderName || '', Validators.required],
          senderCompany: [data.senderCompany || '', Validators.required],
          senderEmail: [data.senderEmail || '', [Validators.required, Validators.email]],
          senderPhone: [data.senderPhone || '', Validators.required],
          senderAddress: [data.senderAddress || '', Validators.required],
          senderAddressCity: [data.senderAddressCity || '', Validators.required],
          senderAddressPostCode: [data.senderAddressPostCode || '', Validators.required],
          senderAddressCountryCode: [data.senderAddressCountryCode || 'PL', Validators.required],
        });
        break;
    }
  }

  back(): void {
    this.selectedType = null;
    this.integrationForm = this.fb.group({});
  }

  submit(): void {
    if (this.selectedType === IntegrationType.Allegro && !this.isEditMode) {
      this.connectAllegro();
      return;
    }

    if (this.integrationForm.invalid) {
      this.integrationForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    const formValue = this.integrationForm.getRawValue(); // getRawValue() pobiera też disabled fields
    const integration = this.data?.integration as any;

    let request;

    if (this.isEditMode) {
      // Tryb edycji
      switch (this.selectedType) {
        case IntegrationType.Erli:
          request = this.integrationsService.updateErliIntegration(integration.sourceSystemId, formValue);
          break;
        case IntegrationType.Shopify:
          request = this.integrationsService.updateShopifyIntegration(integration.sourceSystemId, formValue);
          break;
        case IntegrationType.Fakturownia:
          request = this.integrationsService.updateFakturowniaIntegration(integration.sourceSystemId, formValue);
          break;
        case IntegrationType.Inpost:
          request = this.integrationsService.updateInpostIntegration(integration.organizationId, {
            ...formValue,
            organizationId: parseInt(formValue.organizationId, 10),
          });
          break;
        case IntegrationType.Dpd:
          request = this.integrationsService.updateDpdIntegration(integration.login, {
            ...formValue,
            login: parseInt(formValue.login, 10),
            masterFid: parseInt(formValue.masterFid, 10),
          });
          break;
        default:
          this.loading = false;
          return;
      }
    } else {
      // Tryb dodawania
      switch (this.selectedType) {
        case IntegrationType.Erli:
          request = this.integrationsService.addErliIntegration(formValue);
          break;
        case IntegrationType.Shopify:
          request = this.integrationsService.addShopifyIntegration(formValue);
          break;
        case IntegrationType.Fakturownia:
          request = this.integrationsService.addFakturowniaIntegration(formValue);
          break;
        case IntegrationType.Inpost:
          request = this.integrationsService.addInpostIntegration({
            ...formValue,
            organizationId: parseInt(formValue.organizationId, 10),
          });
          break;
        case IntegrationType.Dpd:
          request = this.integrationsService.addDpdIntegration({
            ...formValue,
            login: parseInt(formValue.login, 10),
            masterFid: parseInt(formValue.masterFid, 10),
          });
          break;
        default:
          this.loading = false;
          return;
      }
    }

    request.subscribe({
      next: () => {
        this.toastr.success(this.isEditMode ? 'Integracja została zaktualizowana' : 'Integracja została dodana');
        this.store.dispatch(new LoadIntegrations());
        this.dialogRef.close(true);
      },
      error: error => {
        this.loading = false;
        this.toastr.error(
          this.isEditMode ? 'Błąd podczas aktualizacji integracji' : 'Błąd podczas dodawania integracji'
        );
        console.error(error);
      },
    });
  }

  connectAllegro(): void {
    this.integrationsService.connectAllegro();
    this.dialogRef.close();
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
