import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MaterialModule } from '../../../../../shared/modules/material.module';

@Component({
  selector: 'app-orders-fixed-buttons',
  templateUrl: './orders-fixed-buttons.component.html',
  styleUrls: ['./orders-fixed-buttons.component.scss'],
  imports: [MaterialModule],
  standalone: true,
})
export class OrdersFixedButtonsComponent {
  private router = inject(Router);

  onCreateOrder(): void {
    this.router.navigate(['/furniture-formats/orders/create']);
  }
}
