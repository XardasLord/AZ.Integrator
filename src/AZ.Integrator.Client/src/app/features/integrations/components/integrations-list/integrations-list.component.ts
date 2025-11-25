import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { IntegrationWithType } from '../../models/integration.model';
import { IntegrationCardComponent } from '../integration-card/integration-card.component';

@Component({
  selector: 'app-integrations-list',
  standalone: true,
  templateUrl: './integrations-list.component.html',
  styleUrls: ['./integrations-list.component.scss'],
  imports: [CommonModule, MaterialModule, IntegrationCardComponent],
})
export class IntegrationsListComponent {
  @Input() integrations: IntegrationWithType[] | null = [];
}
