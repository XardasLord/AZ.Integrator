import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { NgIf } from '@angular/common';
import { FurnitureFormatsRoutePath, RoutePaths } from '../../modules/app-routing.module';
import { environment } from '../../../../environments/environment';
import { AuthRoles } from '../../../shared/auth/models/auth.roles';
import { ToolbarComponent } from '../toolbar/toolbar.component';
import { AuthRoleAllowDirective } from '../../../shared/auth/directives/auth-role-allow.directive';
import { MaterialModule } from '../../../shared/modules/material.module';

export type NavigationItem = {
  title: string;
  icon: string;
  route?: string;
  roles: AuthRoles[];
  subItems?: NavigationItem[];
};

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
  imports: [MaterialModule, ToolbarComponent, RouterLinkActive, RouterLink, NgIf, AuthRoleAllowDirective, RouterOutlet],
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
      roles: [],
    },
    {
      title: 'Zamówienia',
      icon: 'shopping_cart',
      route: RoutePaths.Orders,
      roles: [AuthRoles.Admin],
    },
    {
      title: 'Szablony paczek',
      icon: 'inventory',
      route: RoutePaths.ParcelTemplates,
      roles: [AuthRoles.Admin],
    },
    {
      title: 'Stany magazynowe',
      icon: 'warehouse',
      route: RoutePaths.Stocks,
      roles: [AuthRoles.Admin],
    },
    {
      title: 'Zarządzanie formatkami',
      icon: 'description',
      roles: [AuthRoles.Admin],
      subItems: [
        {
          title: 'Formatki',
          icon: 'folder',
          route: FurnitureFormatsRoutePath.Formats,
          roles: [AuthRoles.Admin],
        },
        {
          title: 'Dostawcy',
          icon: 'folder',
          route: FurnitureFormatsRoutePath.Suppliers,
          roles: [AuthRoles.Admin],
        },
        {
          title: 'Zamówienia',
          icon: 'folder',
          route: FurnitureFormatsRoutePath.Orders,
          roles: [AuthRoles.Admin],
        },
      ],
    },
    {
      title: 'Statystyki',
      icon: 'bar_chart',
      route: RoutePaths.StocksStatistics,
      roles: [],
    },
    {
      title: 'Skanowanie kodów',
      icon: 'qr_code_scanner',
      route: RoutePaths.BarcodeScanner,
      roles: [AuthRoles.ScannerIn, AuthRoles.ScannerOut],
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
