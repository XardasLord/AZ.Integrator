import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Login } from '../../../../shared/states/auth.action';
import { environment } from '../../../../../environments/environment';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    standalone: false
})
export class LoginComponent {
  form: FormGroup;
  constructor(
    private fb: FormBuilder,
    private store: Store
  ) {
    this.form = this.fb.group({
      login: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  login() {
    const val = this.form.value;

    if (val.login && val.password) {
      // this.store.dispatch(new Login(val.login, val.password));
    }
  }

  get appVersion() {
    return environment.version;
  }
}
