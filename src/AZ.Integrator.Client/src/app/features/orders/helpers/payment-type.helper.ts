import { OrderDetailsModel } from '../models/order-details.model';

export function getPaymentTypeForOrder(order: OrderDetailsModel) {
  if (order.payment.type === 'ONLINE') {
    return 'Płatność przelewem';
  }
  if (order.payment.type === 'CASH_ON_DELIVERY') {
    return 'Za pobraniem';
  }

  return order.payment.type;
}
