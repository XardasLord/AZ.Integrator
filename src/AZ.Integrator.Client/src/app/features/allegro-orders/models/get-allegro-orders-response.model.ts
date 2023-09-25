import { AllegroOrderModel } from './allegro-order.model';

export interface GetAllegroOrdersResponseModel {
  orders: AllegroOrderModel[];
  totalCount: number;
}
