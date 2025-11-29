import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { map, Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { MaterialModule } from '../../../../shared/modules/material.module';
import { IntegrationWithType } from '../../models/integration.model';
import { IntegrationsState } from '../../states/integrations.state';
import { LoadIntegrations } from '../../states/integrations.action';
import { IntegrationsListComponent } from '../../components/integrations-list/integrations-list.component';
import { AddIntegrationButtonComponent } from '../../components/add-integration-button/add-integration-button.component';
import { IntegrationsFiltersComponent } from '../../components/integrations-filters/integrations-filters.component';
import { IntegrationCategory, IntegrationTypeCategories } from '../../models/integration-type.enum';

@Component({
  selector: 'app-integrations',
  standalone: true,
  templateUrl: './integrations.component.html',
  styleUrls: ['./integrations.component.scss'],
  imports: [
    CommonModule,
    MaterialModule,
    IntegrationsListComponent,
    AddIntegrationButtonComponent,
    IntegrationsFiltersComponent,
  ],
})
export class IntegrationsComponent implements OnInit {
  private store = inject(Store);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  integrations$: Observable<IntegrationWithType[]>;
  filteredIntegrations$: Observable<IntegrationWithType[]>;
  loading$: Observable<boolean>;

  selectedCategory: IntegrationCategory | null = null;
  searchText = '';

  constructor() {
    this.integrations$ = this.store.select(IntegrationsState.integrations);
    this.loading$ = this.store.select(IntegrationsState.loading);
    this.filteredIntegrations$ = this.integrations$;
  }

  ngOnInit(): void {
    this.store.dispatch(new LoadIntegrations());
    this.checkConnectionStatus();
  }

  private checkConnectionStatus(): void {
    const queryParams = this.route.snapshot.queryParamMap;

    // Sprawdzenie różnych typów połączeń
    const connectionTypes = [
      { param: 'allegro_connected', name: 'Allegro' },
      { param: 'erli_connected', name: 'Erli' },
      { param: 'shopify_connected', name: 'Shopify' },
      { param: 'fakturownia_connected', name: 'Fakturownia' },
      { param: 'inpost_connected', name: 'InPost' },
      { param: 'dpd_connected', name: 'DPD' },
    ];

    const connectedType = connectionTypes.find(type => queryParams.has(type.param));

    if (connectedType) {
      // Pokazanie komunikatu o pomyślnym połączeniu
      this.toastr.success(
        `Twoje konto ${connectedType.name} zostało pomyślnie połączone z systemem`,
        'Połączenie nawiązane! 🎉',
        {
          timeOut: 5000,
          progressBar: true,
          closeButton: true,
        }
      );

      // Usunięcie parametru z URL, aby nie był widoczny
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: {},
        replaceUrl: true,
      });
    }
  }

  onCategoryChange(category: IntegrationCategory | null): void {
    this.selectedCategory = category;
    this.applyFilters();
  }

  onSearchChange(searchText: string): void {
    this.searchText = searchText;
    this.applyFilters();
  }

  private applyFilters(): void {
    this.filteredIntegrations$ = this.integrations$.pipe(
      map(integrations => {
        let filtered = integrations;

        // Filtruj po kategorii
        if (this.selectedCategory) {
          filtered = filtered.filter(
            integration => IntegrationTypeCategories[integration.type] === this.selectedCategory
          );
        }

        // Filtruj po tekście wyszukiwania
        if (this.searchText) {
          const searchLower = this.searchText.toLowerCase();
          filtered = filtered.filter(integration => {
            const displayName = (integration.integration as any).displayName || '';
            return displayName.toLowerCase().includes(searchLower);
          });
        }

        return filtered;
      })
    );
  }

  getActiveCount(): Observable<number> {
    return this.integrations$.pipe(
      map(integrations => integrations.filter(integration => (integration.integration as any).isEnabled).length)
    );
  }
}
