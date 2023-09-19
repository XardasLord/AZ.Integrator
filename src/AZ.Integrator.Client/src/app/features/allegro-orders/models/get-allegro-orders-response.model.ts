import { AllegroOrderModel } from './allegro-order.model';

export interface GetAllegroOrdersResponseModel {
  ordersCount: number;
  orders: AllegroOrderModel[];
}
