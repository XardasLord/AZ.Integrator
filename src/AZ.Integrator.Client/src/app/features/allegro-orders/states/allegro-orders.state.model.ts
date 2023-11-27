import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';
import { ShipmentViewModel } from '../../../shared/graphql/graphql-integrator.schema';

export interface AllegroOrdersStateModel {
  restQuery: RestQueryVo;
  restQueryResponse: RestQueryResponse<AllegroOrderDetailsModel[]>;
  selectedOrderDetails: AllegroOrderDetailsModel | null;
  shipments: ShipmentViewModel[];
  currentTab: 'New' | 'ReadyForShipment' | 'Sent';
}
