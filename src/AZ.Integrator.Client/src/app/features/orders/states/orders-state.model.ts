import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import { OrderDetailsModel } from '../models/order-details.model';
import { ShipmentViewModel } from '../../../shared/graphql/graphql-integrator.schema';

export interface OrdersStateModel {
  restQuery: RestQueryVo;
  restQueryResponse: RestQueryResponse<OrderDetailsModel[]>;
  selectedOrderDetails: OrderDetailsModel | null;
  shipments: ShipmentViewModel[];
  currentTab: 'New' | 'ReadyForShipment' | 'Sent';
}
