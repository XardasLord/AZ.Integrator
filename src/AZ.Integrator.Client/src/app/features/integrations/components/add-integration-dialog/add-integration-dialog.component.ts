import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Store } from '@ngxs/store';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { IntegrationType, IntegrationTypeLabels, IntegrationTypeLogos } from '../../models/integration-type.enum';
import { IntegrationsService } from '../../services/integrations.service';
import { LoadIntegrations } from '../../states/integrations.action';
import { ToastrService } from 'ngx-toastr';

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

  integrationTypes = Object.values(IntegrationType);
  IntegrationType = IntegrationType;

  constructor() {
    this.integrationForm = this.fb.group({});
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

  buildForm(type: IntegrationType): void {
    switch (type) {
      case IntegrationType.Allegro:
        // Allegro używa OAuth, nie potrzebuje formularza
        this.integrationForm = this.fb.group({});
        break;

      case IntegrationType.Erli:
        this.integrationForm = this.fb.group({
          sourceSystemId: ['', Validators.required],
          apiKey: ['', Validators.required],
          displayName: ['', Validators.required],
        });
        break;

      case IntegrationType.Shopify:
        this.integrationForm = this.fb.group({
          sourceSystemId: ['', Validators.required],
          apiUrl: ['', [Validators.required, Validators.pattern(/^https?:\/\/.+/)]],
          adminApiToken: ['', Validators.required],
          displayName: ['', Validators.required],
        });
        break;

      case IntegrationType.Fakturownia:
        this.integrationForm = this.fb.group({
          sourceSystemId: ['', Validators.required],
          apiKey: ['', Validators.required],
          apiUrl: ['', [Validators.required, Validators.pattern(/^https?:\/\/.+/)]],
          displayName: ['', Validators.required],
        });
        break;

      case IntegrationType.Inpost:
        this.integrationForm = this.fb.group({
          organizationId: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
          accessToken: ['', Validators.required],
          displayName: ['', Validators.required],
          senderName: ['', Validators.required],
          senderCompanyName: ['', Validators.required],
          senderFirstName: ['', Validators.required],
          senderLastName: ['', Validators.required],
          senderEmail: ['', [Validators.required, Validators.email]],
          senderPhone: ['', Validators.required],
          senderAddressStreet: ['', Validators.required],
          senderAddressBuildingNumber: ['', Validators.required],
          senderAddressCity: ['', Validators.required],
          senderAddressPostCode: ['', Validators.required],
          senderAddressCountryCode: ['PL', Validators.required],
        });
        break;

      case IntegrationType.Dpd:
        this.integrationForm = this.fb.group({
          login: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
          password: ['', Validators.required],
          masterFid: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
          senderName: ['', Validators.required],
          senderCompany: ['', Validators.required],
          senderEmail: ['', [Validators.required, Validators.email]],
          senderPhone: ['', Validators.required],
          senderAddress: ['', Validators.required],
          senderAddressCity: ['', Validators.required],
          senderAddressPostCode: ['', Validators.required],
          senderAddressCountryCode: ['PL', Validators.required],
        });
        break;
    }
  }

  back(): void {
    this.selectedType = null;
    this.integrationForm = this.fb.group({});
  }

  submit(): void {
    if (this.selectedType === IntegrationType.Allegro) {
      this.connectAllegro();
      return;
    }

    if (this.integrationForm.invalid) {
      this.integrationForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    const formValue = this.integrationForm.value;

    let request;
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

    request.subscribe({
      next: () => {
        this.toastr.success('Integracja została dodana');
        this.store.dispatch(new LoadIntegrations());
        this.dialogRef.close(true);
      },
      error: error => {
        this.loading = false;
        this.toastr.error('Błąd podczas dodawania integracji');
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
