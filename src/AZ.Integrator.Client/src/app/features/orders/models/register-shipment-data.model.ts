import { OrderDetailsModel } from './order-details.model';

export interface RegisterShipmentDataModel {
  order: OrderDetailsModel;
  deliveryMethodType: 'DPD' | 'INPOST';
}
