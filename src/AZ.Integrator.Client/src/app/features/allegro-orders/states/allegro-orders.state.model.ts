import { AllegroOrderModel } from '../models/allegro-order.model';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import { AllegroOrderDetailsModel } from '../models/allegro-order-details.model';

export interface AllegroOrdersStateModel {
  restQuery: RestQueryVo;
  restQueryResponse: RestQueryResponse<AllegroOrderModel[]>;
  selectedOrderDetails: AllegroOrderDetailsModel | null;
}
