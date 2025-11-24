import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngxs/store';
import { MatDialog } from '@angular/material/dialog';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { IntegrationWithType } from '../../models/integration.model';
import { IntegrationType } from '../../models/integration-type.enum';
import { DeleteIntegration, ToggleIntegrationStatus } from '../../states/integrations.action';
import { ConfirmationDialogComponent } from '../../../../shared/components/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-integration-card',
  standalone: true,
  templateUrl: './integration-card.component.html',
  styleUrls: ['./integration-card.component.scss'],
  imports: [CommonModule, MaterialModule],
})
export class IntegrationCardComponent {
  private store = inject(Store);
  private dialog = inject(MatDialog);

  @Input() integrationWithType!: IntegrationWithType;

  get displayName(): string {
    return (this.integrationWithType.integration as any).displayName || 'Bez nazwy';
  }

  get isEnabled(): boolean {
    return this.integrationWithType.integration.isEnabled;
  }

  get sourceSystemId(): string {
    const integration = this.integrationWithType.integration as any;
    return integration.sourceSystemId || integration.organizationId?.toString() || '';
  }

  get createdAt(): Date {
    return new Date((this.integrationWithType.integration as any).createdAt);
  }

  get logoUrl(): string {
    switch (this.integrationWithType.type) {
      case IntegrationType.Allegro:
        return '../../../../../assets/logo/allegro.svg';
      case IntegrationType.Erli:
        return '../../../../../assets/logo/erli.svg';
      case IntegrationType.Shopify:
        return '../../../../../assets/logo/shopify.svg';
      case IntegrationType.Fakturownia:
        return '../../../../../assets/logo/fakturownia.svg';
      case IntegrationType.Inpost:
        return '../../../../../assets/logo/inpost.svg';
      case IntegrationType.Dpd:
        return '../../../../../assets/logo/dpd.svg';
      default:
        return '';
    }
  }

  get statusColor(): string {
    return this.isEnabled ? 'text-green-600' : 'text-red-600';
  }

  get statusLabel(): string {
    return this.isEnabled ? 'Aktywna' : 'Nieaktywna';
  }

  toggleStatus(): void {
    this.store.dispatch(
      new ToggleIntegrationStatus(
        this.integrationWithType.type as IntegrationType,
        this.sourceSystemId,
        !this.isEnabled
      )
    );
  }

  deleteIntegration(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: 'Usuń integrację',
        message: `Czy na pewno chcesz usunąć integrację "${this.displayName}"?`,
        confirmText: 'Usuń',
        cancelText: 'Anuluj',
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.store.dispatch(
          new DeleteIntegration(this.integrationWithType.type as IntegrationType, this.sourceSystemId)
        );
      }
    });
  }

  getIntegrationDetails(): string[] {
    const integration = this.integrationWithType.integration as any;
    const details: string[] = [];

    switch (this.integrationWithType.type) {
      case IntegrationType.Allegro:
        if (integration.sourceSystemId) details.push(`ID: ${integration.sourceSystemId}`);
        break;
      case IntegrationType.Erli:
        if (integration.sourceSystemId) details.push(`System ID: ${integration.sourceSystemId}`);
        break;
      case IntegrationType.Shopify:
        if (integration.apiUrl) details.push(`URL: ${integration.apiUrl}`);
        break;
      case IntegrationType.Fakturownia:
        if (integration.apiUrl) details.push(`URL: ${integration.apiUrl}`);
        break;
      case IntegrationType.Inpost:
        if (integration.organizationId) details.push(`Org ID: ${integration.organizationId}`);
        if (integration.senderName) details.push(`Nadawca: ${integration.senderName}`);
        break;
      case IntegrationType.Dpd:
        if (integration.login) details.push(`Login: ${integration.login}`);
        if (integration.senderCompany) details.push(`Firma: ${integration.senderCompany}`);
        break;
    }

    return details;
  }
}
