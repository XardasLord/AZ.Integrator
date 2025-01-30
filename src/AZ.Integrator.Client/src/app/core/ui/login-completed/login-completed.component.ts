import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngxs/store';
import { LoginCompleted } from '../../../shared/states/auth.action';

@Component({
    selector: 'app-login-completed',
    templateUrl: './login-completed.component.html',
    styleUrl: './login-completed.component.scss',
    standalone: false
})
export class LoginCompletedComponent implements OnInit {
  private store = inject(Store);


  ngOnInit(): void {
    this.store.dispatch(new LoginCompleted());
  }
}
