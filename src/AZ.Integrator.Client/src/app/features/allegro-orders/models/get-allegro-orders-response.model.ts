import { AllegroOrderDetailsModel } from './allegro-order-details.model';

export interface GetAllegroOrdersResponseModel {
  orders: AllegroOrderDetailsModel[];
  count: number;
  totalCount: number;
}
