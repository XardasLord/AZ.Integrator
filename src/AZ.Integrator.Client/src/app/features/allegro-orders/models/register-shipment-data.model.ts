import { AllegroOrderDetailsModel } from './allegro-order-details.model';

export interface RegisterShipmentDataModel {
  allegroOrder: AllegroOrderDetailsModel;
  deliveryMethodType: 'DPD' | 'INPOST';
}
