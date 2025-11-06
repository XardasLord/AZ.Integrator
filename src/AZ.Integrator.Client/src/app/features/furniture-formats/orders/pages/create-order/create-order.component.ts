import { Component } from '@angular/core';
import { CreateOrderFormComponent } from '../../components/create-order-form/create-order-form.component';

@Component({
  selector: 'app-create-order',
  imports: [CreateOrderFormComponent],
  templateUrl: './create-order.component.html',
  styleUrl: './create-order.component.scss',
  standalone: true,
})
export class CreateOrderComponent {}
