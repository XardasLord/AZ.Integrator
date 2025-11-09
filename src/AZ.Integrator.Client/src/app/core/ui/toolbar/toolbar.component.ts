import { Component, EventEmitter, inject, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { AuthState } from '../../../shared/states/auth.state';
import { MaterialModule } from '../../../shared/modules/material.module';
import { AsyncPipe } from '@angular/common';
import Keycloak from 'keycloak-js';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
  imports: [MaterialModule, AsyncPipe],
  standalone: true,
})
export class ToolbarComponent {
  private store = inject(Store);
  private keycloak = inject(Keycloak);

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
