import { Component, EventEmitter, inject, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { KeycloakService } from 'keycloak-angular';
import { AuthState } from '../../../shared/states/auth.state';
import { MaterialModule } from '../../../shared/modules/material.module';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
  imports: [MaterialModule, AsyncPipe],
  standalone: true,
})
export class ToolbarComponent {
  private store = inject(Store);
  private keycloak = inject(KeycloakService);

  @Output()
  toggleSideNav: EventEmitter<boolean> = new EventEmitter();
  user$ = this.store.select(AuthState.getProfile);

  toggleMenu(): void {
    this.toggleSideNav.emit(true);
  }

  logout() {
    this.keycloak.logout();
  }
}
