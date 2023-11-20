import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';

export function getPaymentTypeForAllegroOrder(order: AllegroOrderDetailsModel) {
  if (order.payment.type === 'ONLINE') {
    return 'Płatność przelewem';
  }
  if (order.payment.type === 'CASH_ON_DELIVERY') {
    return 'Za pobraniem';
  }

  return order.payment.type;
}
