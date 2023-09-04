import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { NgxsModule } from '@ngxs/store';
import { ToastrModule } from 'ngx-toastr';
import { MaterialModule } from './modules/material.module';
import { ErrorService } from './errors/error.service';
import { AuthService } from './services/auth.service';
import { AuthScopeAllowDirective } from './auth/directives/auth-scope-allow.directive';
import { DictionaryService } from './services/dictionary.service';
import { DictionaryState } from './states/dictionary.state';
import { ProgressSpinnerComponent } from './components/progress-spinner/progress-spinner.component';
import { ProgressSpinnerService } from './services/progress-spinner.service';

@NgModule({
  declarations: [AuthScopeAllowDirective, ProgressSpinnerComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    HttpClientModule,
    NgxsModule.forFeature([DictionaryState]),
    ToastrModule.forRoot(),
    NgOptimizedImage,
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    HttpClientModule,
    ToastrModule,
    AuthScopeAllowDirective,
    ProgressSpinnerComponent,
    NgOptimizedImage,
  ],
  providers: [
    ErrorService,
    AuthService,
    DictionaryService,
    ProgressSpinnerService,
    { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { duration: 2500 } },
  ],
})
export class SharedModule {
  constructor() {}
}
