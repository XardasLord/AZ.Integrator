import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { provideStates } from '@ngxs/store';
import { ToastrModule } from 'ngx-toastr';
import { MaterialModule } from './modules/material.module';
import { ErrorService } from './errors/error.service';
import { AuthService } from './services/auth.service';
import { AuthScopeAllowDirective } from './auth/directives/auth-scope-allow.directive';
import { DictionaryService } from './services/dictionary.service';
import { DictionaryState } from './states/dictionary.state';
import { ProgressSpinnerComponent } from './components/progress-spinner/progress-spinner.component';
import { ProgressSpinnerService } from './services/progress-spinner.service';
import { DownloadService } from './services/download.service';
import { DebounceDirective } from './directives/debounce.directive';
import { AuthRoleAllowDirective } from './auth/directives/auth-role-allow.directive';

@NgModule({
    imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, ToastrModule.forRoot(), NgOptimizedImage, AuthScopeAllowDirective, AuthRoleAllowDirective, ProgressSpinnerComponent, DebounceDirective],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MaterialModule,
        ToastrModule,
        AuthScopeAllowDirective,
        AuthRoleAllowDirective,
        ProgressSpinnerComponent,
        NgOptimizedImage,
        DebounceDirective,
    ],
    providers: [
        provideStates([DictionaryState]),
        provideHttpClient(withInterceptorsFromDi()),
        ErrorService,
        AuthService,
        DictionaryService,
        ProgressSpinnerService,
        DownloadService,
        { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { duration: 2500 } },
    ],
})
export class SharedModule {
  constructor() {}
}
