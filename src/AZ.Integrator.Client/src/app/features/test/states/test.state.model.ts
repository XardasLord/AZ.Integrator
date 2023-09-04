import { TestModel } from '../models/test.model';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';

export interface TestStateModel {
  restQuery: RestQueryVo;
  restQueryResponse: RestQueryResponse<TestModel[]>;
}
