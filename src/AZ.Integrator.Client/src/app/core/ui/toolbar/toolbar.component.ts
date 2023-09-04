import { Component, EventEmitter, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { Logout } from '../../../shared/states/auth.action';
import { AuthState } from '../../../shared/states/auth.state';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent {
  @Output()
  toggleSideNav: EventEmitter<boolean> = new EventEmitter();
  showAlerts = false;
  user$ = this.store.select(AuthState.getUser);

  constructor(private store: Store) {}

  toggleMenu(): void {
    this.toggleSideNav.emit(true);
  }

  logout(): void {
    this.store.dispatch(new Logout());
  }

  toggleAlerts(): void {
    this.showAlerts = !this.showAlerts;
  }
}
