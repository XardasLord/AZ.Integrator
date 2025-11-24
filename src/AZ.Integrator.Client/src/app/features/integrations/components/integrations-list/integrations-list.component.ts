import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { IntegrationWithType } from '../../models/integration.model';
import { IntegrationCardComponent } from '../integration-card/integration-card.component';
import { IntegrationType, IntegrationTypeLabels } from '../../models/integration-type.enum';

@Component({
  selector: 'app-integrations-list',
  standalone: true,
  templateUrl: './integrations-list.component.html',
  styleUrls: ['./integrations-list.component.scss'],
  imports: [CommonModule, MaterialModule, IntegrationCardComponent],
})
export class IntegrationsListComponent {
  @Input() integrations: IntegrationWithType[] | null = [];

  get groupedIntegrations(): Map<IntegrationType, IntegrationWithType[]> {
    const grouped = new Map<IntegrationType, IntegrationWithType[]>();

    if (!this.integrations) return grouped;

    // Inicjalizuj wszystkie typy integracji
    Object.values(IntegrationType).forEach(type => {
      grouped.set(type, []);
    });

    // Grupuj integracje
    this.integrations.forEach(integration => {
      const existing = grouped.get(integration.type) || [];
      grouped.set(integration.type, [...existing, integration]);
    });

    return grouped;
  }

  getIntegrationTypeLabel(type: IntegrationType): string {
    return IntegrationTypeLabels[type];
  }

  getIntegrationTypesArray(): IntegrationType[] {
    return Object.values(IntegrationType);
  }
}
