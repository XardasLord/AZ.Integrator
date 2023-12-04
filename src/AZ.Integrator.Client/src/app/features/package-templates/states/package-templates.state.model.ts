import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';

export interface PackageTemplatesStateModel {
  restQuery: RestQueryVo;
  restQueryResponse: RestQueryResponse<string[]>;
}
