import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import {
  FurnitureFormatsRoutePath,
  IntegrationsRoutePath,
  MarketplaceRoutePath,
  RoutePaths,
  StocksRoutePath,
} from '../../modules/app-routing.module';
import { environment } from '../../../../environments/environment';
import { AllRoles, AuthRoles } from '../../../shared/auth/models/auth.roles';
import { ToolbarComponent } from '../toolbar/toolbar.component';
import { AuthRoleAllowDirective } from '../../../shared/auth/directives/auth-role-allow.directive';
import { MaterialModule } from '../../../shared/modules/material.module';
import { FeatureFlagDirective } from '../../../shared/feature-flags/directives/feature-flag.directive';
import {
  FeatureFlagCode_IntegrationsModule,
  FeatureFlagCode_Marketplace_OrdersModule,
  FeatureFlagCode_Marketplace_ParcelTemplatesModule,
  FeatureFlagCode_MarketplaceModule,
  FeatureFlagCode_ProcurementModule,
  FeatureFlagCode_Warehouse_ScanningBarcodesModule,
  FeatureFlagCode_Warehouse_StatisticsModule,
  FeatureFlagCode_Warehouse_StocksModule,
  FeatureFlagCode_WarehouseModule,
} from '../../../shared/feature-flags/models/feature-flags-codes.model';

export type NavigationItem = {
  title: string;
  icon: string;
  route?: string;
  roles: AuthRoles[];
  subItems?: NavigationItem[];
  featureFlag?: string;
};

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
  imports: [
    MaterialModule,
    ToolbarComponent,
    RouterLinkActive,
    RouterLink,
    AuthRoleAllowDirective,
    RouterOutlet,
    FeatureFlagDirective,
  ],
})
export class NavigationComponent implements OnInit {
  @ViewChild('sidenav') sidenav!: MatSidenav;
  sidenavMode: 'side' | 'over' = 'side';
  sidenavOpened = true;
  expandedItems: Set<string> = new Set();

  AuthRoles = AuthRoles;

  navigationItems: NavigationItem[] = [
    {
      title: 'Strona główna',
      icon: 'home',
      route: RoutePaths.Home,
      roles: AllRoles,
    },

    {
      title: 'Sklep i wysyłka',
      icon: 'storefront',
      roles: [AuthRoles.Admin, AuthRoles.Operator],
      featureFlag: FeatureFlagCode_MarketplaceModule,
      subItems: [
        {
          title: 'Zamówienia',
          icon: 'shopping_cart',
          route: MarketplaceRoutePath.Orders,
          roles: [AuthRoles.Admin, AuthRoles.Operator],
          featureFlag: FeatureFlagCode_Marketplace_OrdersModule,
        },
        {
          title: 'Szablony paczek',
          icon: 'inventory',
          route: MarketplaceRoutePath.ParcelTemplates,
          roles: [AuthRoles.Admin, AuthRoles.Operator],
          featureFlag: FeatureFlagCode_Marketplace_ParcelTemplatesModule,
        },
      ],
    },

    {
      title: 'Magazyn',
      icon: 'warehouse',
      roles: [AuthRoles.Admin, AuthRoles.Operator],
      featureFlag: FeatureFlagCode_WarehouseModule,
      subItems: [
        {
          title: 'Stany magazynowe',
          icon: 'inventory',
          route: StocksRoutePath.Stocks,
          roles: [AuthRoles.Admin, AuthRoles.Operator],
          featureFlag: FeatureFlagCode_Warehouse_StocksModule,
        },
        {
          title: 'Skanowanie kodów',
          icon: 'qr_code_scanner',
          route: StocksRoutePath.BarcodeScanner,
          roles: [AuthRoles.ScannerIn, AuthRoles.ScannerOut],
          featureFlag: FeatureFlagCode_Warehouse_ScanningBarcodesModule,
        },
        {
          title: 'Statystyki',
          icon: 'bar_chart',
          route: StocksRoutePath.Statistics,
          roles: [AuthRoles.Admin],
          featureFlag: FeatureFlagCode_Warehouse_StatisticsModule,
        },
      ],
    },
    {
      title: 'Zarządzanie formatkami',
      icon: 'description',
      roles: [AuthRoles.Admin, AuthRoles.Operator],
      featureFlag: FeatureFlagCode_ProcurementModule,
      subItems: [
        {
          title: 'Formatki',
          icon: 'view_module',
          route: FurnitureFormatsRoutePath.Formats,
          roles: [AuthRoles.Admin, AuthRoles.Operator],
          featureFlag: FeatureFlagCode_ProcurementModule,
        },
        {
          title: 'Dostawcy',
          icon: 'factory',
          route: FurnitureFormatsRoutePath.Suppliers,
          roles: [AuthRoles.Admin, AuthRoles.Operator],
          featureFlag: FeatureFlagCode_ProcurementModule,
        },
        {
          title: 'Zamówienia',
          icon: 'assignment',
          route: FurnitureFormatsRoutePath.Orders,
          roles: [AuthRoles.Admin, AuthRoles.Operator],
          featureFlag: FeatureFlagCode_ProcurementModule,
        },
      ],
    },
    {
      title: 'Integracje',
      icon: 'settings_input_component',
      route: IntegrationsRoutePath.Integrations,
      roles: [AuthRoles.Admin],
      featureFlag: FeatureFlagCode_IntegrationsModule,
    },
  ];

  get appVersion() {
    return environment.version;
  }

  ngOnInit(): void {
    this.updateSidenavMode();
  }

  @HostListener('window:resize', [])
  onResize() {
    this.updateSidenavMode();
  }

  toggleExpand(item: NavigationItem): void {
    if (item.subItems && item.subItems.length > 0) {
      if (this.expandedItems.has(item.title)) {
        this.expandedItems.delete(item.title);
      } else {
        this.expandedItems.add(item.title);
      }
    }
  }

  isExpanded(item: NavigationItem): boolean {
    return this.expandedItems.has(item.title);
  }

  hasSubItems(item: NavigationItem): boolean {
    return !!item.subItems && item.subItems.length > 0;
  }

  private updateSidenavMode() {
    if (window.innerWidth < 768) {
      this.sidenavMode = 'over';
      this.sidenavOpened = false;
    } else {
      this.sidenavMode = 'side';
      this.sidenavOpened = true;
    }
  }
}
