import { OrderDetailsModel } from './order-details.model';

export interface RegisterShipmentDataModel {
  allegroOrder: OrderDetailsModel;
  deliveryMethodType: 'DPD' | 'INPOST';
}
