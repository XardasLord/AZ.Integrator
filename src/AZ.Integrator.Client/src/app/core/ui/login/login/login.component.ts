import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { environment } from '../../../../../environments/environment';
import { MaterialModule } from '../../../../shared/modules/material.module';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [MaterialModule, FormsModule, ReactiveFormsModule],
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private store = inject(Store);

  form: FormGroup;
  constructor() {
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
