import { OrderDetailsModel } from './order-details.model';

export interface GetOrdersResponseModel {
  orders: OrderDetailsModel[];
  count: number;
  totalCount: number;
}
