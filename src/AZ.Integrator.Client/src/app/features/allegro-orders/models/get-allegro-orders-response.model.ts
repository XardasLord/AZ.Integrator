import { OrderDetailsModel } from './order-details.model';

export interface GetAllegroOrdersResponseModel {
  orders: OrderDetailsModel[];
  count: number;
  totalCount: number;
}
