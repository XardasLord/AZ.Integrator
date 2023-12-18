import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';

export interface ParcelTemplatesStateModel {
  restQuery: RestQueryVo;
  restQueryResponse: RestQueryResponse<string[]>;
}
