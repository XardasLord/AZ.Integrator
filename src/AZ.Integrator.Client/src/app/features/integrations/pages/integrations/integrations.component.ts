import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { IntegrationWithType } from '../../models/integration.model';
import { IntegrationsState } from '../../states/integrations.state';
import { LoadIntegrations } from '../../states/integrations.action';
import { IntegrationsListComponent } from '../../components/integrations-list/integrations-list.component';
import { AddIntegrationButtonComponent } from '../../components/add-integration-button/add-integration-button.component';

@Component({
  selector: 'app-integrations',
  standalone: true,
  templateUrl: './integrations.component.html',
  styleUrls: ['./integrations.component.scss'],
  imports: [CommonModule, MaterialModule, IntegrationsListComponent, AddIntegrationButtonComponent],
})
export class IntegrationsComponent implements OnInit {
  integrations$: Observable<IntegrationWithType[]>;
  loading$: Observable<boolean>;

  constructor(private store: Store) {
    this.integrations$ = this.store.select(IntegrationsState.integrations);
    this.loading$ = this.store.select(IntegrationsState.loading);
  }

  ngOnInit(): void {
    this.store.dispatch(new LoadIntegrations());
  }
}
