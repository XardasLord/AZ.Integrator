import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { LoginCompleted } from '../../../shared/states/auth.action';

@Component({
  selector: 'app-login-completed',
  templateUrl: './login-completed.component.html',
  styleUrl: './login-completed.component.scss',
})
export class LoginCompletedComponent implements OnInit {
  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new LoginCompleted());
  }
}
