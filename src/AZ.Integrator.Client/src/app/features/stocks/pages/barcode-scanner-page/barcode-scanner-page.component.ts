import { Component } from '@angular/core';
import { AuthRoles } from '../../../../shared/auth/models/auth.roles';
import { BarcodeScannerComponent } from '../barcode-scanner/barcode-scanner.component';
import { SharedModule } from '../../../../shared/shared.module';

@Component({
  selector: 'app-barcode-scanner-page',
  imports: [SharedModule, BarcodeScannerComponent],
  templateUrl: './barcode-scanner-page.component.html',
  styleUrl: './barcode-scanner-page.component.scss',
  standalone: true,
})
export class BarcodeScannerPageComponent {
  protected readonly AuthRoles = AuthRoles;
}
